using System.Collections.Generic;
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

        [HttpGet]
        public async Task<ActionResult<List<Payment>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        //[HttpPut("{id}")]
        //public async Task<ActionResult<Payment>> UpdatePayment(int id, Payment payment)
        //{
        //    if (id != payment.Id)
        //        return BadRequest("Payment ID mismatch.");

        //    var updatedPayment = await _paymentService.UpdatePayment(payment);
        //    if (updatedPayment == null)
        //        return NotFound();

        //    return Ok(updatedPayment);
        //}

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePayment(int id)
        {
            var success = await _paymentService.DeletePayment(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
