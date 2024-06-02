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

        [HttpGet("ongoing-rental-single/{id}")]
        public async Task<IActionResult> OngoingRentalSingle(int id)
        {
            var ongoingRentalSingle = await _frontReservationServices.OngoingRentalSingle(id);

            return Ok(ongoingRentalSingle);
        }

        [HttpGet("rental-history/{id}")]
        public async Task<IActionResult> RentalHistory(int id)
        {
            var rentalHistory = await _frontReservationServices.RentalHistory(id);

            return Ok(rentalHistory);
        }

        [HttpGet("rental-history-single/{id}")]
        public async Task<IActionResult> RentalHistorySingle(int id)
        {
            var rentalHistorySingle = await _frontReservationServices.RentalHistorySingle(id);

            return Ok(rentalHistorySingle);
        }
    }
}
