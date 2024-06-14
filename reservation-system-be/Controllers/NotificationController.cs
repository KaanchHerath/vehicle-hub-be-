using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Models;
using reservation_system_be.Services.NotificationServices;

namespace reservation_system_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        /*[HttpGet("Notifications/{uid}")]
        public async Task<ActionResult<List<Notification>>> GetNotifications(int uid)
        {
            try
            {
                var notifications = await _notificationService.GetAllNotifications(uid);
                if (notifications == null || notifications.Count == 0)
                {
                    return NotFound();
                }
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/

        [HttpPost("AddNotification")]
        public async Task<ActionResult<Notification>> AddNotification(Notification notification)
        {
            try
            {
                var newNotification = await _notificationService.AddNotification(notification);
                return Ok(newNotification);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
