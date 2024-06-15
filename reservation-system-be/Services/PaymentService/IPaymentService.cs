using reservation_system_be.DTOs;
using reservation_system_be.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace reservation_system_be.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<PaymentServiceDTO?> AddPayment(Payment payment);
        Task<List<PaymentServiceDTO>> GetAllPayments();
        Task<PaymentServiceDTO?> GetPaymentById(int id);
        Task<bool> DeletePayment(int id);
    }
}
