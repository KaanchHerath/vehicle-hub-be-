using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Services.ReservationPendingService
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

                    var invoice = await context.Invoices
                        .FirstOrDefaultAsync(i => i.CustomerReservationId == pendingCustomerReservation.Id);

                    var customerReservation = await customerReservationService.GetCustomerReservation(pendingCustomerReservation.Id);

                    if (invoice != null && (DateTime.UtcNow - invoice.DateCreated).TotalDays > 3)
                    {
                        MailRequest mailRequest = new MailRequest
                        {
                            ToEmail = customerReservation.Customer.Email,
                            Subject = "Pending Reservation has been cancelled",
                            Body = DepositPaymentMissedNotificationMail(customerReservation.Id)
                        };
                        await emailService.SendEmailAsync(mailRequest);

                        reservation.Status = Status.Cancelled;
                        await reservationService.UpdateReservation(reservation.Id, reservation);
                    }
                }
            }
        }

        private string DepositPaymentMissedNotificationMail(int reservationId)
        {
            string contactLink = "http://localhost:3000/contact";

            string response = @"
                <!DOCTYPE html>
                <html>
                  <head>
                    <meta charset='UTF-8' />
                    <title>Reservation Cancellation Due to Missed Deposit Payment</title>
                  </head>
                  <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                    <div style='text-align: center; margin-bottom: 20px'>
                      <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                    </div>
                    <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 550px;'>
                      <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Reservation Cancellation</h1>
                      <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #" + reservationId + @"</h2>
                      <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                      <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>We regret to inform you that your reservation has been cancelled due to non-receipt of the deposit payment within the required 3-day period.</p>
                      <p style='color: #000000; text-align: left; margin: 5px 0;'>As outlined in our reservation policy, a deposit payment is necessary to confirm your reservation. Unfortunately, we did not receive your payment within the specified timeframe, resulting in the cancellation of your reservation.</p>
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
