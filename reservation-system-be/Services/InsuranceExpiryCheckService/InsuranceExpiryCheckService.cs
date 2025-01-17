﻿using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.AdminNotificationServices;
using reservation_system_be.Services.NotificationServices;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.InsuranceExpiryCheckService
{
    public class InsuranceExpiryCheckService : IHostedService, IDisposable
    {
        private readonly ILogger<InsuranceExpiryCheckService> _logger;
        public readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public InsuranceExpiryCheckService(ILogger<InsuranceExpiryCheckService> logger, IServiceProvider serviceProvider)
        { 
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Insurance Expiry Check Service running.");

            _timer = new Timer(CheckExpiries, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void CheckExpiries(object state)
        {
            _logger.LogInformation("Checking insurance expiries.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var adminNotificationService = scope.ServiceProvider.GetRequiredService<IAdminNotificationService>();
                var vehicleService = scope.ServiceProvider.GetRequiredService<IVehicleService>();

                var expiringInsurances = await context.VehicleInsurances
                .Where(vi => vi.ExpiryDate <= DateTime.Now.AddDays(7))
                .ToListAsync(); // Fetch the data first with ToListAsync

                expiringInsurances = expiringInsurances
                    .Where(e => e.Status)
                    .ToList();

                foreach (var insurance in expiringInsurances)
                {
                    var vehicle = await vehicleService.GetVehicle(insurance.VehicleId);
                    var adminNotification = new AdminNotification
                    {
                        Type = "Insurance",
                        Title = "Insurance Expiry",
                        Description = $"Insurance for vehicle {vehicle.RegistrationNumber} is expiring on {insurance.ExpiryDate.ToString("yyyy-MM-dd")}",
                        Generated_DateTime = DateTime.Now,
                        IsRead = false
                    };

                    if(context.AdminNotifications.Any(n => n.Description == adminNotification.Description))
                    {
                        continue;
                    }
                    await adminNotificationService.AddAdminNotification(adminNotification);
                }

                var vehicles = await context.Vehicles.ToListAsync();

                foreach (var vehicle in vehicles)
                {
                    var vehicleInsurance = await context.VehicleInsurances
                        .Where(vi => vi.VehicleId == vehicle.Id)
                        .OrderByDescending(vi => vi.ExpiryDate)
                        .FirstOrDefaultAsync();

                    if (vehicleInsurance == null || vehicleInsurance.Status == false)
                    {
                        var adminNotification = new AdminNotification
                        {
                            Type = "Insurance",
                            Title = "Uninsured Vehicle",
                            Description = $"Vehicle {vehicle.RegistrationNumber} currently does not have insurance coverage for the rest of {DateTime.Now.Year}.",
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
            _logger.LogInformation("Insurance Expiry Check Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
