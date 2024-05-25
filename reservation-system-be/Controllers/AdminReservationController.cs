using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("{id}/{eid}")]
        public async Task<IActionResult> AcceptReservation(int id, int eid)
        {
            await _adminReservationService.AcceptReservation(id, eid);
            return Ok();
        }

        [HttpPost("Begin-Reservation/{id}")]
        public async Task<IActionResult> BeginReservation(int id)
        {
            await _adminReservationService.BeginReservation(id);
            return Ok();
        }
    }
}
