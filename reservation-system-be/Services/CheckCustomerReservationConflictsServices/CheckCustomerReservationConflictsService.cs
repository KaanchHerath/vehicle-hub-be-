using reservation_system_be.Data;
using reservation_system_be.Models;
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
                var customerReservationService = scope.ServiceProvider.GetRequiredService<ICustomerReservationService>();
                var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();


                var inactiveVehicles = context.Vehicles
                    .Where(v => !v.Status)
                    .ToList();

                foreach (var vehicle in inactiveVehicles)
                {
                    var customerReservations = context.CustomerReservations
                        .Where(cr => cr.VehicleId == vehicle.Id && (cr.Reservation.Status != Status.Ongoing || cr.Reservation.Status != Status.Ended || 
                                    cr.Reservation.Status != Status.Completed || cr.Reservation.Status != Status.Cancelled) && cr.Reservation.StartDate > DateTime.Now)
                        .ToList();

                    foreach (var customerReservation in customerReservations)
                    {
                        var newVehicle = await vehicleService.GetVehicle(customerReservation.VehicleId);
                        var notification = new Notification
                        {
                            Type = "Reservation Conflicts",
                            Title = "Vehicle Unavailable",
                            Description = $"The vehicle for the reservation {customerReservation.Id} with registration number {newVehicle.VehicleModel.VehicleMake.Name} {newVehicle.VehicleModel.Name} is currently unavailable ",
                            Generated_DateTime = DateTime.Now,
                            CustomerReservationId = customerReservation.Id
                        };

                        if (context.Notifications.Any(n => n.Description == notification.Description))
                        {
                            continue;
                        }
                        await notificationService.AddNotification(notification);
                    }
                }

                await context.SaveChangesAsync();
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