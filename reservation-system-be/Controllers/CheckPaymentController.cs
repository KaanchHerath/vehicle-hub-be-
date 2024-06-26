using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Services.CheckPaymentService;
using System.Threading.Tasks;

namespace reservation_system_be.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckPaymentController : Controller
    {
        private readonly ICheckPaymentService _checkPaymentService;

        public CheckPaymentController(ICheckPaymentService checkPaymentService)
        {
            _checkPaymentService = checkPaymentService;
        }

        [HttpGet("CheckPayment/{invoiceId}")]
        public async Task<IActionResult> CheckPayment(int invoiceId)
        {
            var result = await _checkPaymentService.CheckPaymentForInvoice(invoiceId);
            if (result.PaymentExists)
            {
                return Ok(result);
            }
            return NotFound("Payment not found for the given invoice ID.");
        }
    }
}
