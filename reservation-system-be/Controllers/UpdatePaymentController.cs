using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Models;
using reservation_system_be.Services.PaymentService;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
        {
            var createdPayment = await _paymentService.AddPayment(payment);
            return CreatedAtAction(nameof(CreatePayment), new { id = createdPayment.Id }, createdPayment);
        }
    }
}

