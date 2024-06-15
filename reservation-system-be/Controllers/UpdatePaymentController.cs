using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.PaymentService;

namespace reservation_system_be.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentServiceDTO>> CreatePayment(Payment payment)
        {
            var createdPayment = await _paymentService.AddPayment(payment);
            if (createdPayment == null)
                return BadRequest("Error creating payment");

            // Use the Id from the created payment to generate the URI
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment?.Id }, createdPayment);
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentServiceDTO>>> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPayments();
            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentServiceDTO>> GetPaymentById(int id)
        {
            var payment = await _paymentService.GetPaymentById(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

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
