using Azure;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Services.EmailServices;

namespace reservation_system_be.Services.FrontReservationServices
{
    public class FrontReservationServices : IFrontReservationServices
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;
        private readonly IEmailService _emailService;
        private readonly ICustomerService _customerService;
        public FrontReservationServices(DataContext context, ICustomerReservationService customerReservationService, IEmailService emailService, ICustomerService customerService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _emailService = emailService;
            _customerService = customerService;
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
            string response = "<div style=\"width:100%;background-color:#f4f4f4;text-align:center;margin:10px;padding:10px;font-family:Arial, sans-serif;\">";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;\">";
            response += "<h1>VehicleHub</h1>";
            response += "</div>";
            response += "<div style=\"margin:20px;text-align:left;\">";
            response += "<img src=\"https://drive.google.com/uc?export=view&id=1S40qYUDb_f9YRAaQeQmPETz5ABYbI32p\" alt=\"Company Logo\" style=\"width:150px;height:auto;display:block;margin:auto;\"/>";
            response += "<h2 style=\"text-align:center;\">Thank you for your request!</h2>";
            response += "<p>We have received your request for the vehicle. Here are the details:</p>";
            response += "<p><strong>Registration Number:</strong> " + registrationNumber + "</p>";
            response += "<p><strong>Car Name:</strong> " + carName + "</p>";
            response += "<p><strong>Pickup Date & Time:</strong> " + pickupDateTime + "</p>";
            response += "<p><strong>Dropoff Date & Time:</strong> " + dropoffDateTime + "</p>";
            response += "<p style=\"margin-top:20px;\">We appreciate your business and look forward to serving you.</p>";
            response += "<p>Best regards,</p>";
            response += "<p><strong>VehicleHub Team</strong></p>";
            response += "</div>";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;margin-top:20px;text-align:center;\">";
            response += "<p>Contact us: info@vehiclehub.com | (123) 456-7890</p>";
            response += "<p>1234 Main St, Anytown, USA</p>";
            response += "</div>";
            response += "</div>";
            return response;
        }

        public async Task<IEnumerable<OngoingRentalDto>> OngoingRentals(int id) // Customer ID
        {
            var customer = await _customerService.GetCustomer(id);
            var ongoingRentals = new List<OngoingRentalDto>();

            var customerReservations = await _context.CustomerReservations.Where(cr => cr.CustomerId == id).ToListAsync();
            if (customerReservations == null)
            {
                throw new DataNotFoundException("No ongoing rentals found");
            }

            foreach (var cr in customerReservations)
            {
                if (cr.Reservation.EndDate > DateTime.Now)
                {
                    var ongoingRental = new OngoingRentalDto
                    {
                        CustomerReservationId = cr.Id,
                        ModelName = cr.Vehicle.VehicleModel.Name,
                        StartDate = cr.Reservation.StartDate,
                        EndDate = cr.Reservation.EndDate,
                        Status = cr.Reservation.Status
                    };
                    ongoingRentals.Add(ongoingRental);
                }
            }

            return ongoingRentals;
        }

        public async Task<OngoingRentalSingleDto> OngoingRentalSingle(int id) // CustomerReservation ID
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);
            var ongoingRentalSingle = new OngoingRentalSingleDto
            {
                CustomerReservationId = customerReservation.Id,
                Name = customerReservation.Customer.Name,
                ModelName = customerReservation.Vehicle.VehicleModel.Name,
                StartDate = customerReservation.Reservation.StartDate,
                EndDate = customerReservation.Reservation.EndDate,
                StartTime = customerReservation.Reservation.StartTime,
                EndTime = customerReservation.Reservation.EndTime,
                Status = customerReservation.Reservation.Status
            };

            return ongoingRentalSingle;
        }
    }
}
