using reservation_system_be.Models;

namespace reservation_system_be.Services.AdminNotificationServices
{
    public interface IAdminNotificationService
    {
        Task<AdminNotification> AddAdminNotification(AdminNotification adminNotification);

        Task<List<AdminNotification>> GetAdminNotifications();

        Task<bool> DeleteAdminNotification(int notificationId);

        Task<bool> MarkAsRead(int notificationId);
    }
}
