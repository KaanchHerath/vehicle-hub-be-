using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Services.CustomerSideServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerSide : ControllerBase
    {
        private readonly ICustomerSideServices _customerSideServices;

        public CustomerSide(ICustomerSideServices customerSideServices)
        {
            _customerSideServices = customerSideServices;
        }

        [HttpPost("request-reservation")]
        public async Task<IActionResult> RequestReservation([FromBody] CreateCustomerReservationDto customerReservationDto)
        {
            var customerReservation = await _customerSideServices.RequestReservations(customerReservationDto);

            return Ok(customerReservation);
        }
    }
}
