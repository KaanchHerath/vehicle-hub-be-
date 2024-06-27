using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.AdminNotificationServices;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.NotificationServices;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.CheckCustomerReservationConflictsServices
{
    public class CheckCustomerReservationConflictsService : IHostedService, IDisposable
    {
        private readonly ILogger<CheckCustomerReservationConflictsService> _logger;
        public readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public CheckCustomerReservationConflictsService(ILogger<CheckCustomerReservationConflictsService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Customer Notifications Service running.");

            _timer = new Timer(SendNotifications, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        private async void SendNotifications(object state)
        {
            _logger.LogInformation("Sending customer notifications.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var adminNotificationService = scope.ServiceProvider.GetRequiredService<IAdminNotificationService>();
                var customerReservationService = scope.ServiceProvider.GetRequiredService<ICustomerReservationService>();
                var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();


                var inactiveVehicles = context.Vehicles
                    .Where(v => !v.Status)
                    .ToList();

                foreach (var vehicle in inactiveVehicles)
                {
                    var customerReservations = context.CustomerReservations
                        .Where(cr => cr.VehicleId == vehicle.Id)
                        .ToList();

                    foreach (var customerReservation in customerReservations)
                    {
                        var cr = await customerReservationService.GetCustomerReservation(customerReservation.Id);
                        if ((cr.Reservation.Status != Status.Pending || cr.Reservation.Status != Status.Confirmed) && cr.Reservation.StartDate < DateTime.Now)
                        {
                            continue;
                        }

                        var newVehicle = await vehicleService.GetVehicle(customerReservation.VehicleId);

                        var notification = new Notification
                        {
                            Type = "Reservation Conflicts",
                            Title = "Vehicle Unavailable",
                            Description = $"The {newVehicle.VehicleModel.VehicleMake.Name} {newVehicle.VehicleModel.Name} is unavailable for your reservation: {customerReservation.Id} on {customerReservation.Reservation.StartDate.ToString("yyyy-MM-dd")}.",
                            Generated_DateTime = DateTime.Now,
                            CustomerReservationId = customerReservation.Id,
                            IsRead = false
                        };

                        if (context.Notifications.Any(n => n.Description == notification.Description))
                        {
                            continue;
                        }
                        await notificationService.AddNotification(notification);

                        var adminNotification = new AdminNotification
                        {
                            Type = "Reservation Conflicts",
                            Title = "Vehicle Unavailable",
                            Description = $"The vehicle {newVehicle.RegistrationNumber} is unavailable for reservation: {customerReservation.Id} on {customerReservation.Reservation.StartDate.ToString("yyyy-MM-dd")}.",
                            Generated_DateTime = DateTime.Now,
                            IsRead = false
                        };

                        if (context.AdminNotifications.Any(n => n.Description == adminNotification.Description))
                        {
                            continue;
                        }
                        await adminNotificationService.AddAdminNotification(adminNotification);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Customer Notifications Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}