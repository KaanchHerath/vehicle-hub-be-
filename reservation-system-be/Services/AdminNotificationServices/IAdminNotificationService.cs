using reservation_system_be.Models;

namespace reservation_system_be.Services.AdminNotificationServices
{
    public interface IAdminNotificationService
    {
        Task<AdminNotification> AddAdminNotification(AdminNotification adminNotification);
    }
}
