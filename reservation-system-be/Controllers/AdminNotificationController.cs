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
    }
}
