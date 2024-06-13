using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly DataContext _context;

        public PaymentService(DataContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<List<Payment>> GetAllPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment> GetPaymentById(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        //public async Task<Payment> UpdatePayment(Payment payment)
        //{
        //    var existingPayment = await _context.Payments.FindAsync(payment.Id);
        //    if (existingPayment == null)
        //        return null;

        //    _context.Entry(existingPayment).CurrentValues.SetValues(payment);
        //    await _context.SaveChangesAsync();

        //    return existingPayment;
        //}

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
