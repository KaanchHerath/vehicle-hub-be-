using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Services.AdminReservationServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminReservationController : ControllerBase
    {
        private readonly IAdminReservationService _adminReservationService;

        public AdminReservationController(IAdminReservationService adminReservationService)
        {
            _adminReservationService = adminReservationService;
        }

        [HttpGet("View-Reservations")]
        public async Task<IActionResult> ViewReservations()
        {
            var reservations = await _adminReservationService.ViewReservations();
            return Ok(reservations);
        }

        [HttpPost("Accept-Reservation/{id}")]
        public async Task<IActionResult> AcceptReservation(int id, int eid)
        {
            await _adminReservationService.AcceptReservation(id, eid);
            return Ok();
        }

        [HttpPost("Decline-Reservation/{id}")]
        public async Task<IActionResult> DeclineReservation(int id, int eid)
        {
            await _adminReservationService.DeclineReservation(id, eid);
            return Ok();
        }

        [HttpPost("Begin-Reservation/{id}")]
        public async Task<IActionResult> BeginReservation(int id, int eid)
        {
            await _adminReservationService.BeginReservation(id, eid);
            return Ok();
        }

        [HttpPost("End-Reservation/{id}")]
        public async Task<IActionResult> EndReservation(int id, int eid, VehicleLogDto vehicleLog)
        {
            await _adminReservationService.EndReservation(id, eid, vehicleLog);
            return Ok();
        }

        [HttpGet("Available-Vehicles/{id}")]
        public async Task<IActionResult> AvailableVehicles(int id)
        {
            var vehicles = await _adminReservationService.AvailableVehicles(id);
            return Ok(vehicles);
        }

        [HttpPost("Change-Vehicle/{id}")]
        public async Task<IActionResult> ReservationChangeVehicle(int id, int vid)
        {
            await _adminReservationService.ReservationChangeVehicle(id, vid);
            return Ok();
        }

        [HttpPost("Cancel-Reservation/{id}")]
        public async Task<IActionResult> CancelReservation(int id, int eid)
        {
            await _adminReservationService.CancelReservation(id, eid);
            return Ok();
        }

        [HttpGet("Customer-Details/{id}")]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            var customer = await _adminReservationService.CustomerDetails(id);
            return Ok(customer);
        }
    }
}
