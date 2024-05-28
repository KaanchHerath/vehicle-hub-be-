using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Services.StripeService;
using System.Threading.Tasks;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IStripeService _stripeService;

        public PaymentsController(IStripeService stripeService)
        {
            _stripeService = stripeService;
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentRequest request)
        {
            var paymentIntent = await _stripeService.CreatePaymentIntent(request.Amount, request.Currency);
            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
    }

    public class PaymentIntentRequest
    {
        public long Amount { get; set; }
        public string Currency { get; set; }
    }
}
