using reservation_system_be.DTOs;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using reservation_system_be.Data;

namespace reservation_system_be.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly DataContext _context;

        public PaymentService(DataContext context)
        {
            _context = context;
        }

        
public async Task<PaymentServiceDTO?> AddPayment(PaymentServiceDTO paymentDto)
        {
            var payment = new Payment
            {
                // Map properties from DTO to Payment entity
                PaymentStatus = paymentDto.PaymentStatus,
                PaymentMethod = paymentDto.PaymentMethod,
                PaymentDate = paymentDto.PaymentDate,
                PaymentTime = paymentDto.PaymentTime,
                InvoiceId = paymentDto.InvoiceId,
                // Assuming you have a way to set ReservationStatus in Payment entity
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Update the ReservationStatus in related entities if necessary

            return await GetPaymentDTOById(payment.Id); // Return DTO
        }

        public async Task<List<PaymentServiceDTO>> GetAllPayments()
        {
            return await _context.Payments
                .Include(p => p.Invoice)
                .ThenInclude(i => i.CustomerReservation)
                .Select(p => new PaymentServiceDTO(
                    p.Id,
                    p.PaymentStatus,
                    p.PaymentMethod,
                    p.PaymentDate,
                    p.PaymentTime,
                    p.InvoiceId,
                    p.Invoice.CustomerReservation != null ? p.Invoice.CustomerReservation.Reservation.Status : Status.Waiting
                )).ToListAsync();
        }

        public async Task<PaymentServiceDTO?> GetPaymentById(int id)
        {
            return await GetPaymentDTOById(id);
        }

        private async Task<PaymentServiceDTO?> GetPaymentDTOById(int id)
        {
            var payment = await _context.Payments
                .Include(p => p.Invoice)
                .ThenInclude(i => i.CustomerReservation)
                .Where(p => p.Id == id)
                .Select(p => new PaymentServiceDTO(
                    p.Id,
                    p.PaymentStatus,
                    p.PaymentMethod,
                    p.PaymentDate,
                    p.PaymentTime,
                    p.InvoiceId,
                    p.Invoice.CustomerReservation != null ? p.Invoice.CustomerReservation.Reservation.Status : Status.Waiting
                )).FirstOrDefaultAsync();

            return payment;
        }

        public async Task<bool> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
                return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
