using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Models;
using reservation_system_be.Services.AdminNotificationServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminNotificationController : ControllerBase
    {
        private readonly IAdminNotificationService _adminNotificationService;
        public AdminNotificationController(IAdminNotificationService adminNotificationService)
        {
            _adminNotificationService = adminNotificationService;
        }

        [HttpPost]
        public async Task<ActionResult<AdminNotification>> AddAdminNotification(AdminNotification adminNotification)
        {
            var newAdminNotification = await _adminNotificationService.AddAdminNotification(adminNotification);
            return Ok(newAdminNotification);
        }

        [HttpGet("AllAdminNotifications")]
        public async Task<ActionResult<List<AdminNotification>>> GetNotifications()
        {
            try
            {
                var notifications = await _adminNotificationService.GetAdminNotifications();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteAdminNotification")]
        public async Task<ActionResult<bool>> DeleteNotification(int notificationId)
        {
            try
            {
                bool isDeleted = await _adminNotificationService.DeleteAdminNotification(notificationId);
                return Ok(isDeleted);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("MarkAsRead")]
        public async Task<ActionResult<bool>> MarkAsRead(int notificationid)
        {
            try
            {
                bool isUpdated = await _adminNotificationService.MarkAsRead(notificationid);
                return Ok(isUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
