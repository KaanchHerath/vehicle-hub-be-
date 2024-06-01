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
    }
}
