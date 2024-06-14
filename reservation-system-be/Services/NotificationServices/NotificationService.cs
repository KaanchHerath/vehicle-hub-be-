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
        /*public async Task<List<Notification>> GetAllNotifications(int uid)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Join(_context.Reservations,
                          notification => notification.ReservationId,
                          reservation => reservation.Id,
                          (notification, reservation) => new { notification, reservation })
                    .Join(_context.CustomerReservations,
                          nr => nr.reservation.Id,
                          customerReservation => customerReservation.ReservationId,
                          (nr, customerReservation) => new { nr.notification, nr.reservation, customerReservation })
                    .Where(nrc => nrc.customerReservation.CustomerId == uid)
                    .Select(nrc => nrc.notification)
                    .ToListAsync();

                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting notifications for user.", ex);
            }
        }*/

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
