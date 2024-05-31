using Microsoft.AspNetCore.Mvc;
using reservation_system_be.DTOs;
using reservation_system_be.Services.ReservationStatusServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationStatusController : Controller
    {
        private readonly IReservationStatusService _rsc;

        public ReservationStatusController(IReservationStatusService rsc)
        {
            _rsc = rsc;
        }

        [HttpGet]
        public async Task<ActionResult> GetReservationCounts()
        {
            
            try
            {
                var result = await _rsc.GetAllReservationStatus();

                var response = new List<object>
                {
                    new { name = "Total Hired", value = result.ConfirmedCount },
                    new { name = "Total Canceled", value = result.CancelledCount },
                    new { name = "Total Pending", value = result.PendingCount }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
