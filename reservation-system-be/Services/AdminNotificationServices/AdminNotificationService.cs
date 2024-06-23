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
    }
}
