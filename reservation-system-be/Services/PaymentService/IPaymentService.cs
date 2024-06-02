using System.Threading.Tasks;
using reservation_system_be.Models;

namespace reservation_system_be.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Payment> AddPayment(Payment payment);
    }
}
