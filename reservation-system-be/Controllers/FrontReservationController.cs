using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> RequestReservation([FromBody] CreateCustomerReservationDto customerReservationDto)
        {
            var customerReservation = await _frontReservationServices.RequestReservations(customerReservationDto);

            return Ok(customerReservation);
        }

        [HttpGet("ongoing-rentals/{id}")]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> OngoingRentals(int id)
        {
            var ongoingRentals = await _frontReservationServices.OngoingRentals(id);

            return Ok(ongoingRentals);
        }

        [HttpGet("ongoing-rental-single/{id}")]
        [Authorize(Policy = "CustomerOnly")]
        public async Task<IActionResult> OngoingRentalSingle(int id)
        {
            var ongoingRentalSingle = await _frontReservationServices.OngoingRentalSingle(id);

            return Ok(ongoingRentalSingle);
        }

        [HttpGet("rental-history/{id}")]
        [Authorize(Policy ="CustomerOnly")]
        public async Task<IActionResult> RentalHistory(int id)
        {
            var rentalHistory = await _frontReservationServices.RentalHistory(id);

            return Ok(rentalHistory);
        }

        [HttpGet("rental-history-single/{id}")]
        [Authorize(Policy ="CustomerOnly")]
        public async Task<IActionResult> RentalHistorySingle(int id)
        {
            var rentalHistorySingle = await _frontReservationServices.RentalHistorySingle(id);

            return Ok(rentalHistorySingle);
        }

        [HttpGet("view-booking-confirmation/{id}")]
        public async Task<IActionResult> ViewBookingConfirmation(int id)
        {
            var bookingConfirmation = await _frontReservationServices.ViewBookingConfirmation(id);

            return Ok(bookingConfirmation);
        }

        [HttpGet("DetailCar/{id}")]
        public async Task<IActionResult> GetVehicleDetails(int id)
        {
            var detailCar = await _frontReservationServices.GetVehicleDetails(id);

            return Ok(detailCar);
        }

        [HttpPost("cancel-reservation/{id}")]
        [Authorize(Policy ="CustomerOnly")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            await _frontReservationServices.CancelReservation(id);

            return Ok();
        }
    }
}
