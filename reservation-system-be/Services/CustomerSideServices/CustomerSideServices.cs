using Azure;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmailServices;

namespace reservation_system_be.Services.CustomerSideServices
{
    public class CustomerSideServices : ICustomerSideServices
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;
        private readonly IEmailService _emailService;
        public CustomerSideServices(DataContext context, ICustomerReservationService customerReservationService, IEmailService emailService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _emailService = emailService;
        }

        public async Task<CustomerReservation> RequestReservations(CreateCustomerReservationDto customerReservationDto)
        {
            var customerReservation = await _customerReservationService.CreateCustomerReservation(customerReservationDto);

            var cr = await _customerReservationService.GetCustomerReservation(customerReservation.Id);
            MailRequest mailRequest = new MailRequest
            {
                ToEmail = cr.Customer.Email,
                Subject = "VehicleHub - Reservation Request",
                Body = RequestMail(cr.Vehicle.RegistrationNumber, cr.Vehicle.VehicleModel.Name, cr.Reservation.StartDate, cr.Reservation.EndDate)
            };
            await _emailService.SendEmailAsync(mailRequest);

            return customerReservation;
        }

        private string RequestMail(string registrationNumber, string carName, DateTime pickupDateTime, DateTime dropoffDateTime)
        {
            string response = "<div style=\"width:100%;background-color:#fbdac6;text-align:center;margin:10px;padding:10px;font-family:Arial, sans-serif;\">";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;\">";
            response += "<h1>VehicleHub</h1>";
            response += "</div>";
            response += "<div style=\"margin:20px;\">";
            response += "<img src=\"D:\\UoM\\Software Project\\Main\\Back-end\\vehicle-hub-be\\reservation-system-be\\Assets\\Blue-Icon.png\" alt=\"Company Logo\" style=\"width:150px;height:auto;\"/>";
            response += "<h2>Thank you for your request!</h2>";
            response += "<p>We have received your request for the vehicle. Here are the details:</p>";
            response += "<table style=\"width:80%;margin:auto;border-collapse:collapse;\">";
            response += "<tr><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">Registration Number</td><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">" + registrationNumber + "</td></tr>";
            response += "<tr><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">Car Name</td><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">" + carName + "</td></tr>";
            response += "<tr><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">Pickup Date & Time</td><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">" + pickupDateTime + "</td></tr>";
            response += "<tr><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">Dropoff Date & Time</td><td style=\"padding:8px;border:1px solid #283280;background-color:#ffffff;color:#283280;\">" + dropoffDateTime + "</td></tr>";
            response += "</table>";
            response += "<p style=\"margin-top:20px;\">We appreciate your business and look forward to serving you.</p>";
            response += "<p>Best regards,</p>";
            response += "<p><strong>VehicleHub Team</strong></p>";
            response += "</div>";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;margin-top:20px;\">";
            response += "<p>Contact us: info@vehiclehub.com | (123) 456-7890</p>";
            response += "<p>1234 Main St, Anytown, USA</p>";
            response += "</div>";
            response += "</div>";
            return response;
        }
    }
}
