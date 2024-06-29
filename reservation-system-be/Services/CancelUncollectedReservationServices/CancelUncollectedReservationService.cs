using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.AdminNotificationServices;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.NotificationServices;
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
                var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                var uncollectedReservations = await context.Reservations
                    .Where(r => r.StartDate < DateTime.Now && r.Status == Status.Pending || r.Status == Status.Confirmed)
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
                        Description = $"Dear {customerReservation.Customer.Name}, your reservation for {customerReservation.Vehicle.VehicleModel.VehicleMake.Name} {customerReservation.Vehicle.VehicleModel.Name} has been cancelled as the vehicle was not collected on {customerReservation.Reservation.StartDate.ToString("yyyy-MM-dd")}.",
                        Generated_DateTime = DateTime.Now,
                        CustomerReservationId = customerReservation.Id,
                        IsRead = false
                    };
                    await notificationService.AddNotification(notification);

                    MailRequest mailRequest = new MailRequest
                    {
                        ToEmail = customerReservation.Customer.Email,
                        Subject = "Reservation Cancelled",
                        Body = VehicleNotCollectedNotificationMail(customerReservation.Id)
                    };
                    await emailService.SendEmailAsync(mailRequest);
                }
            }
        }

        private string VehicleNotCollectedNotificationMail(int reservationId)
        {
            string contactLink = "http://localhost:3000/contact";

            string response = @"
                <!DOCTYPE html>
                <html>
                  <head>
                    <meta charset='UTF-8' />
                    <title>Reservation Cancellation Due to Non-Collection</title>
                  </head>
                  <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                    <div style='text-align: center; margin-bottom: 20px'>
                      <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                    </div>
                    <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 550px;'>
                      <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Reservation Cancellation Notice</h1>
                      <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #" + reservationId + @"</h2>
                      <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                      <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>We regret to inform you that your reservation has been cancelled due to the non-collection of the reserved vehicle on the agreed date.</p>
                      <p style='color: #000000; text-align: left; margin: 5px 0;'>As per our policy, it is essential to collect the vehicle on the scheduled date to confirm your reservation. Unfortunately, we did not receive any communication or attempt to collect the vehicle within the agreed timeframe, leading to the cancellation.</p>
                      <p style='color: #000000; text-align: left; margin: 5px 0;'>If you believe this is an error or if you have any questions, please do not hesitate to contact us. We are here to assist you and can provide further information or help you with rebooking.</p>
                      <div style='text-align: center; margin: 40px auto'>
                        <a href='" + contactLink + @"' style='background-color: #283280; border: none; color: #fbdac6; padding: 15px 20px; text-decoration: none; border-radius: 5px; font-size: small;'>Contact Us</a>
                      </div>
                      <p style='color: #000000; text-align: left; margin-top: 20px;'>We apologize for any inconvenience this may have caused and appreciate your understanding.</p>
                      <p style='color: #000000; text-align: left; margin-bottom: 5px;'>Best regards,</p>
                      <p style='color: #000000; text-align: left; margin-top: 5px;'><strong>VehicleHub Team</strong></p>
                      <p style='padding: 10px; margin-top: 40px; text-align: center;'>Contact us: <a href='mailto:vehiclehub01@gmail.com'>vehiclehub01@gmail.com</a> | <a href='tel:+94771234567'>+94 77 123 4567</a></p>
                    </div>
                    <div style='text-align: center; margin-top: 20px; color: #7f7f7f;'>
                      <p style='font-size: 12px;'><strong>All rights reserved @VehicleHub. " + DateTime.Now.Year + @"</strong></p>
                      <p style='font-size: 12px;'>1234 Galle Road, Colombo, Sri Lanka</p>
                    </div>
                  </body>
                </html>
                ";

            return response;
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
}
