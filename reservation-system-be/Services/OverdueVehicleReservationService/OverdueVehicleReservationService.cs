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
                        Body = LateVehicleReturnNotificationMail(customerReservation.Id)
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

        private string LateVehicleReturnNotificationMail(int reservationId)
        {
            string contactLink = "http://localhost:3000/contact";

            string response = @"
                <!DOCTYPE html>
                <html>
                  <head>
                    <meta charset='UTF-8' />
                    <title>Urgent: Late Vehicle Return Notice</title>
                  </head>
                  <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                    <div style='text-align: center; margin-bottom: 20px'>
                      <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                    </div>
                    <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 550px;'>
                      <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Late Vehicle Return Notice</h1>
                      <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #" + reservationId + @"</h2>
                      <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                      <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>We noticed that the vehicle for your reservation has not been returned by the agreed end date.</p>
                      <p style='color: #000000; text-align: left; margin: 5px 0;'>It is important that you contact us immediately to arrange for the return of the vehicle. Please be aware that failure to return the vehicle on time may result in additional fines and penalties as per our policy.</p>
                      <p style='color: #000000; text-align: left; margin: 5px 0;'>Your prompt attention to this matter is highly appreciated.</p>
                      <div style='text-align: center; margin: 40px auto'>
                        <a href='" + contactLink + @"' style='background-color: #283280; border: none; color: #fbdac6; padding: 15px 20px; text-decoration: none; border-radius: 5px; font-size: small;'>Contact Us</a>
                      </div>
                      <p style='color: #000000; text-align: left; margin-top: 20px;'>We apologize for any inconvenience this may have caused and appreciate your immediate action on this matter.</p>
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
