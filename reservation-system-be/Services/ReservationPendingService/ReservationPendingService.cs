using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Services.NewFolder
{
    public class ReservationPendingService : IHostedService, IDisposable
    {
        private readonly ILogger<ReservationPendingService> _logger;
        public readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public ReservationPendingService(ILogger<ReservationPendingService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reservation Pending Service running.");
            _timer = new Timer(CheckReservations, null, TimeSpan.Zero, TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }
        private async void CheckReservations(object state)
        {
            _logger.LogInformation("Checking reservation pending.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
                var customerReservationService = scope.ServiceProvider.GetRequiredService<ICustomerReservationService>();

                var pendingReservations = await context.Reservations
                    .Where(r => r.Status == Status.Pending)
                    .ToListAsync();

                foreach (var reservation in pendingReservations)
                {
                    var pendingCustomerReservation = await context.CustomerReservations
                        .FirstOrDefaultAsync(cr => cr.ReservationId == reservation.Id);

                    if (pendingCustomerReservation == null)
                    {
                        continue;
                    }

                    var invoice = context.Invoices
                        .FirstOrDefault(i => i.CustomerReservationId == pendingCustomerReservation.Id);

                    var customerReservation = customerReservationService.GetCustomerReservation(pendingCustomerReservation.Id);

                    if (invoice != null && (DateTime.UtcNow - invoice.DateCreated).TotalDays > 3)
                    {
                        MailRequest mailRequest = new MailRequest
                        {
                            ToEmail = customerReservation.Result.Customer.Email,
                            Subject = "Pending Reservation has been cancelled",
                            Body = $"Your reservation is pending for more than 3 days. Please pay the invoice to confirm the reservation."
                        };
                        await emailService.SendEmailAsync(mailRequest);

                        reservation.Status = Status.Cancelled;

                        await reservationService.UpdateReservation(reservation.Id, reservation);
                    }
                }
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reservation Pending Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
