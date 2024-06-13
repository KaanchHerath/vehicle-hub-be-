using System.Collections.Generic;
using System.Threading.Tasks;
using reservation_system_be.Models;

namespace reservation_system_be.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Payment> AddPayment(Payment payment);
        Task<List<Payment>> GetAllPayments(); // New method
        Task<Payment> GetPaymentById(int id); // New method
        //Task<Payment> UpdatePayment(Payment payment); // New method
        Task<bool> DeletePayment(int id); // New method
    }
}
