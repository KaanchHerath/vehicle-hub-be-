using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Services.AdminPanelServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPanelController : ControllerBase
    {
        private readonly IAdminPanelService _adminPanelService;

        public AdminPanelController(IAdminPanelService adminPanelService)
        {
            _adminPanelService = adminPanelService;
        }

        [HttpGet("View-Reservations")]
        public async Task<IActionResult> ViewReservations()
        {
            var reservations = await _adminPanelService.ViewReservations();
            return Ok(reservations);
        }

        [HttpPost("{id}/{eid}")]
        public async Task<IActionResult> AcceptReservation(int id, int eid)
        {
            await _adminPanelService.AcceptReservation(id, eid);
            return Ok();
        }
    }
}
