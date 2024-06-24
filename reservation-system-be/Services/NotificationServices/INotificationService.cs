using reservation_system_be.Models;

namespace reservation_system_be.Services.NotificationServices
{
    public interface INotificationService
    {
        Task<List<Notification>> GetAllNotifications(int uid);

        Task<List<Notification>> GetNotifications();

        Task<bool> DeleteNotification(int notificationId);

        Task<Notification> AddNotification(Notification notification);

        Task<bool> MarkAsRead(int notificationId);
    }

}
