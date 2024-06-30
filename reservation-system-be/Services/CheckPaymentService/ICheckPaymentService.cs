using reservation_system_be.DTOs;
using System.Threading.Tasks;

namespace reservation_system_be.Services.CheckPaymentService
{
    public interface ICheckPaymentService
    {
        Task<PaymentCheckResponseDto> CheckPaymentForInvoice(int invoiceId);
    }
}
