using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Services.CancelUncollectedReservationServices
{
    public class CancelUncollectedReservationService : IHostedService, IDisposable
    {
        private readonly ILogger<CancelUncollectedReservationService> _logger;
        public readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public CancelUncollectedReservationService(ILogger<CancelUncollectedReservationService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cancel Uncollected Reservation Service running.");
            _timer = new Timer(CancelUncollectedReservations, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }
        private async void CancelUncollectedReservations(object state)
        {
            _logger.LogInformation("Checking uncollected reservations.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var customerReservationService = scope.ServiceProvider.GetRequiredService<ICustomerReservationService>();
                var reservationService = scope.ServiceProvider.GetRequiredService<IReservationService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                var uncollectedReservations = await context.Reservations
                    .Where(r => r.StartDate > DateTime.Now && r.Status == Status.Pending || r.Status == Status.Confirmed)
                    .ToListAsync();

                foreach (var reservation in uncollectedReservations)
                {
                    reservation.Status = Status.Cancelled;
                    await reservationService.UpdateReservation(reservation.Id, reservation);

                    var cr = await context.CustomerReservations
                        .FirstOrDefaultAsync(cr => cr.ReservationId == reservation.Id);
                    if (cr == null)
                    {
                        continue;
                    }

                    var customerReservation = await customerReservationService.GetCustomerReservation(cr.Id);

                    var notification = new Notification
                    {
                        Type = "Reservation",
                        Title = "Reservation Cancelled",
                        Description = "Your reservation has been cancelled due to uncollected vehicle.",
                        Generated_DateTime = DateTime.Now,
                        CustomerReservationId = customerReservation.Id,
                        IsRead = false
                    };
                    await context.Notifications.AddAsync(notification);

                    MailRequest mailRequest = new MailRequest
                    {
                        ToEmail = customerReservation.Customer.Email,
                        Subject = "Reservation Cancelled",
                        Body = "Your reservation has been cancelled due to uncollected vehicle."
                    };
                    await emailService.SendEmailAsync(mailRequest);
                }
            }
        }
        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cancel Uncollected Reservation Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
    {

    }
}
