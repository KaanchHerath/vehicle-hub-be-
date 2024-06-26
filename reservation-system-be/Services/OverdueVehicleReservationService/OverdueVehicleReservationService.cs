using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.AdminNotificationServices;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Services.OverdueVehicleReservationService
{
    public class OverdueVehicleReservationService: IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OverdueVehicleReservationService> _logger;

        public OverdueVehicleReservationService(IServiceProvider serviceProvider, ILogger<OverdueVehicleReservationService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OverdueVehicleReservationService is starting.");
            _timer = new Timer(OverdueVehicleReservationsAsync, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private async void OverdueVehicleReservationsAsync(object state)
        {
            _logger.LogInformation("OverdueVehicleReservationService is working.");
            using var scope = _serviceProvider.CreateScope();
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var customerReservationService = scope.ServiceProvider.GetRequiredService<ICustomerReservationService>();
                var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var adminNotificationService = scope.ServiceProvider.GetRequiredService<IAdminNotificationService>();

                var unreturnedReservations = await context.Reservations
                    .Where(r => r.EndDate < DateTime.Now && r.Status == Status.Ongoing)
                    .ToListAsync();

                foreach (var reservation in unreturnedReservations)
                { 
                    var cr = await context.CustomerReservations
                        .FirstOrDefaultAsync(cr => cr.ReservationId == reservation.Id);

                    if (cr == null)
                    {
                        continue;
                    }

                    var customerReservation = await customerReservationService.GetCustomerReservation(cr.Id);

                    var adminNotification = new AdminNotification
                    {
                        Type = "Reservation",
                        Title = "Unreturned vehicle",
                        Description = $"The vehicle with Registration No.{customerReservation.Vehicle.RegistrationNumber} rented by {customerReservation.Customer.Name} was not returned as scheduled on {reservation.EndDate.ToString("yyyy-MM-dd")}.",
                        Generated_DateTime = DateTime.Now,
                        IsRead = false
                    };
                    
                    MailRequest mailRequest = new MailRequest
                    {
                        ToEmail = customerReservation.Customer.Email,
                        Subject = "Unreturned vehicle",
                        Body = $"Dear {customerReservation.Customer.Name},\n\nOur records indicate that you have not returned the vehicle as scheduled. Please contact us immediately to arrange for its return.\n\nThank you"
                    };

                    if (context.AdminNotifications.Any(n => n.Description == adminNotification.Description))
                    {
                        continue;
                    }
                    await adminNotificationService.AddAdminNotification(adminNotification);
                    await emailService.SendEmailAsync(mailRequest);
                }
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("OverdueVehicleReservationService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
