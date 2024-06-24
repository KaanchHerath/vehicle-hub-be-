using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.AdminNotificationServices
{
    public class AdminNotificationService : IAdminNotificationService
    {
        private readonly DataContext _context;
        public AdminNotificationService(DataContext context)
        {
            _context = context;
        }

        public async Task<AdminNotification> AddAdminNotification(AdminNotification adminNotification)
        {
            var an = new AdminNotification
            {
                Type = adminNotification.Type,
                Title = adminNotification.Title,
                Description = adminNotification.Description,
                Generated_DateTime = DateTime.Now,
                IsRead = false
            };

            //_context.AdminNotifications.Add(adminNotification);
            await _context.SaveChangesAsync();

            return adminNotification;
        }

        public async Task<List<AdminNotification>> GetAdminNotifications()
        {
            try
            {
                var notifications = await _context.AdminNotifications
                                                  .OrderByDescending(n => n.Generated_DateTime)
                                                  .ToListAsync();
                return notifications;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting notifications.", ex);
            }
        }

        public async Task<bool> DeleteAdminNotification(int notificationId)
        {
            try
            {
                var notification = await _context.AdminNotifications.FindAsync(notificationId);
                if (notification == null)
                {
                    return false;
                }

                _context.AdminNotifications.Remove(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting notification.", ex);
            }
        }

        public async Task<bool> MarkAsRead(int notificationId)
        {
            try
            {
                var notification = await _context.AdminNotifications.FindAsync(notificationId);

                if (notification == null)
                {
                    throw new Exception("Notification not found.");
                }

                notification.IsRead = true;
                _context.AdminNotifications.Update(notification);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating notification.", ex);
            }
        }
    }
}
