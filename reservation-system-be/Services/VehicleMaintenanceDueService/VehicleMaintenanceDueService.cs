using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.NotificationServices;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.VehicleMaintenanceDueService
{
    public class VehicleMaintenanceDueService: IHostedService, IDisposable
    {
        private readonly ILogger<VehicleMaintenanceDueService> _logger;
        public readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public VehicleMaintenanceDueService(ILogger<VehicleMaintenanceDueService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vehicle Maintenance Due Service running.");

            _timer = new Timer(CheckDueMaintenance, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void CheckDueMaintenance(object state)
        {
            _logger.LogInformation("Checking vehicle maintenance due.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();

                var vehicles = context.Vehicles
                    .Where(v => v.Status)
                    .ToList();

                foreach (var vehicle in vehicles)
                {
                    var lastService = context.VehicleMaintenances
                        .Where(vm => vm.VehicleId == vehicle.Id && vm.Type == "service")
                        .OrderByDescending(vm => vm.Date)
                        .FirstOrDefault();

                    if (lastService != null && lastService.CurrentMileage + 5000 <= vehicle.Mileage)
                    {
                        var notification = new Notification
                        {
                            Type = "Maintenance",
                            Title = "Vehicle Service Maintenance Due",
                            Description = $"Vehicle {vehicle.RegistrationNumber} is due for Service.",
                            Generated_DateTime = DateTime.Now,
                            VehicleMaintenanceId = lastService.Id
                        };

                        if (context.Notifications.Any(n => n.VehicleMaintenanceId == lastService.Id))
                        {
                            continue;
                        }
                        await notificationService.AddNotification(notification);
                    }
                }
                foreach (var vehicle in vehicles)
                {
                    var lastBrakePadReplacement = context.VehicleMaintenances
                        .Where(vm => vm.VehicleId == vehicle.Id && vm.Type == "brakePadReplacement")
                        .OrderByDescending(vm => vm.Date)
                        .FirstOrDefault();

                    if (lastBrakePadReplacement != null && lastBrakePadReplacement.CurrentMileage + 15000 <= vehicle.Mileage)
                    {
                        var notification = new Notification
                        {
                            Type = "Maintenance",
                            Title = "Vehicle Break Pad Replacement Maintenance Due",
                            Description = $"Vehicle {vehicle.RegistrationNumber} is due for Brake Pad Replacement Maintenance.",
                            Generated_DateTime = DateTime.Now,
                            VehicleMaintenanceId = lastBrakePadReplacement.Id
                        };

                        /*if (context.Notifications.Any(n => n.VehicleMaintenanceId == lastBrakePadReplacement.Id))
                        {
                            continue;
                        }
                        await notificationService.AddNotification(notification);*/
                    }    
                }
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Vehicle Maintenance Due Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Change(Timeout.Infinite, 0);
        }
    }
}
