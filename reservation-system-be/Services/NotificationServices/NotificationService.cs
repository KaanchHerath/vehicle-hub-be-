using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.NotificationServices
{
    public class NotificationService : INotificationService
    {
        private readonly DataContext _context;

        public NotificationService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Notification>> GetAllNotifications(int uid)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Join(_context.CustomerReservations,
                          notification => notification.CustomerReservationId,
                          customerReservation => customerReservation.Id,
                          (notification, customerReservation) => new { notification, customerReservation })
                    .Where(nrc => nrc.customerReservation.CustomerId == uid)
                    .Select(nrc => nrc.notification)
                    .ToListAsync();

                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting notifications for user.", ex);
            }
        }

        public async Task<List<Notification>> GetNotifications()
        {
            try
            {
                var notifications = await _context.Notifications
                                                  .OrderByDescending(n => n.Generated_DateTime)
                                                  .ToListAsync();
                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting notifications for user.", ex);
            }
        }

        public async Task<bool> DeleteNotification(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification == null)
                {
                    return false;
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting notification.", ex);
            }
        }

        public async Task<Notification> AddNotification(Notification notification)
        {
            try
            {
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                return notification;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding notification.", ex);
            }
        }

    }
}
