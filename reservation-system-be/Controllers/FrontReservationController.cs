using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Services.FrontReservationServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrontReservationService : ControllerBase
    {
        private readonly IFrontReservationServices _frontReservationServices;

        public FrontReservationService(IFrontReservationServices frontReservationServices)
        {
            _frontReservationServices = frontReservationServices;
        }

        [HttpPost("request-reservation")]
        public async Task<IActionResult> RequestReservation([FromBody] CreateCustomerReservationDto customerReservationDto)
        {
            var customerReservation = await _frontReservationServices.RequestReservations(customerReservationDto);

            return Ok(customerReservation);
        }

        [HttpGet("ongoing-rentals/{id}")]
        public async Task<IActionResult> OngoingRentals(int id)
        {
            var ongoingRentals = await _frontReservationServices.OngoingRentals(id);

            return Ok(ongoingRentals);
        }
    }
}
