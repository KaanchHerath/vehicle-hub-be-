using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.FeedbackServices;

namespace reservation_system_be.Services.NotificationService
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
        }

    }
}
