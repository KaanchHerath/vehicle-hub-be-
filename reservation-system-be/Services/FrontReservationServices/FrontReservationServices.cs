using Azure;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Migrations;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.InvoiceService;

namespace reservation_system_be.Services.FrontReservationServices
{
    public class FrontReservationServices : IFrontReservationServices
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;
        private readonly IEmailService _emailService;
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;
        public FrontReservationServices(DataContext context, ICustomerReservationService customerReservationService, IEmailService emailService,
            ICustomerService customerService, IInvoiceService invoiceService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _emailService = emailService;
            _customerService = customerService;
            _invoiceService = invoiceService;
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

        public async Task<IEnumerable<RentalDto>> OngoingRentals(int id) // Customer ID
        {
            var customerReservations = await _customerReservationService.GetAllCustomerReservations();
            if (customerReservations == null)
            {
                throw new DataNotFoundException("No ongoing rentals found");
            }

            customerReservations = customerReservations.Where(cr => cr.Customer.Id == id)
                .OrderByDescending(cr => cr.Reservation.StartDate)
                .ToList();

            var ongoingRentals = new List<RentalDto>();

            foreach (var cr in customerReservations)
            {
                if ((cr.Reservation.Status == Status.Waiting) || (cr.Reservation.Status == Status.Pending) || (cr.Reservation.Status == Status.Confirmed) || 
                    (cr.Reservation.Status == Status.Ongoing) || (cr.Reservation.Status == Status.Ended))
                {
                    var ongoingRental = new RentalDto
                    {
                        CustomerReservationId = cr.Id,
                        ModelName = cr.Vehicle.VehicleModel.Name,
                        Make = cr.Vehicle.VehicleModel.VehicleMake.Name,
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
                ModelName = customerReservation.Vehicle.VehicleModel.Name,
                Make = customerReservation.Vehicle.VehicleModel.VehicleMake.Name,
                StartDate = customerReservation.Reservation.StartDate,
                EndDate = customerReservation.Reservation.EndDate,
                StartTime = customerReservation.Reservation.StartTime,
                EndTime = customerReservation.Reservation.EndTime,
                Status = customerReservation.Reservation.Status,
                Thumbnail = customerReservation.Vehicle.Thumbnail
            };

            return ongoingRentalSingle;
        }

        public async Task<IEnumerable<RentalDto>> RentalHistory(int id) // Customer ID
        {
            var customerReservations = await _customerReservationService.GetAllCustomerReservations();
            if (customerReservations == null)
            {
                throw new DataNotFoundException("No ongoing rentals found");
            }

            customerReservations = customerReservations.Where(cr => cr.Customer.Id == id)
                .OrderByDescending(cr => cr.Reservation.EndDate)
                .ToList();

            var rentalHistorys = new List<RentalDto>();

            foreach (var cr in customerReservations)
            {
                if ((cr.Reservation.Status == Status.Completed) || (cr.Reservation.Status == Status.Cancelled))
                {
                    var rentalHistory = new RentalDto
                    {
                        CustomerReservationId = cr.Id,
                        ModelName = cr.Vehicle.VehicleModel.Name,
                        Make = cr.Vehicle.VehicleModel.VehicleMake.Name,
                        StartDate = cr.Reservation.StartDate,
                        EndDate = cr.Reservation.EndDate,
                        Status = cr.Reservation.Status
                    };
                    rentalHistorys.Add(rentalHistory);
                }
            }

            return rentalHistorys;
        }

        public async Task<RentalHistorySingleDto> RentalHistorySingle(int id) // CustomerReservation ID
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);
            var amount = 0.0f; var extraKMCost = 0.0f; var penalty = 0.0f; var rentalCost = 0.0f;

            if (customerReservation.Reservation.Status == Status.Completed)
            {
                rentalCost = customerReservation.Vehicle.CostPerDay * customerReservation.Reservation.NoOfDays;
                var vehicleLog = await _context.VehicleLogs.FirstOrDefaultAsync(vl => vl.CustomerReservationId == id);
                if (vehicleLog == null)
                {
                    throw new DataNotFoundException("No rental history found");
                }
                extraKMCost = vehicleLog.ExtraKM * customerReservation.Vehicle.CostPerExtraKM;
                penalty = vehicleLog.Penalty;

                var invoice = await _context.Invoices.Where(i => i.Type == "Final").FirstOrDefaultAsync(i => i.CustomerReservationId == id);
                if (invoice == null)
                {
                    throw new DataNotFoundException("No rental history found");
                }
                amount = invoice.Amount;
            }
            else if (customerReservation.Reservation.Status == Status.Cancelled)
            {
                // Intentionally left blank for cancelled reservations
            }
            else
            {
                throw new DataNotFoundException("No rental history found");
            }

            var rentalHistorySingle = new RentalHistorySingleDto
            {
                CustomerReservationId = customerReservation.Id,
                ModelName = customerReservation.Vehicle.VehicleModel.Name,
                Make = customerReservation.Vehicle.VehicleModel.VehicleMake.Name,
                StartDate = customerReservation.Reservation.StartDate,
                EndDate = customerReservation.Reservation.EndDate,
                StartTime = customerReservation.Reservation.StartTime,
                EndTime = customerReservation.Reservation.EndTime,
                Status = customerReservation.Reservation.Status,
                Deposit = customerReservation.Vehicle.VehicleType.DepositAmount,
                RentalCost = rentalCost,
                ExtraKMCost = extraKMCost,
                Penalty = penalty,
                Amount = amount,
                Thumbnail = customerReservation.Vehicle.Thumbnail
            };

            return rentalHistorySingle;
        }

        public async Task<BookingConfirmationDto> ViewBookingConfirmation(int id) // Invoice ID
        {
            var invoice = await _invoiceService.GetInvoiceById(id);
            if (invoice == null)
            {
                throw new DataNotFoundException("No booking confirmation found");
            }

            var customerReservation = await _customerReservationService.GetCustomerReservation(invoice.CustomerReservationId);


            var vehicleLog = await _context.VehicleLogs.FirstOrDefaultAsync(vl => vl.CustomerReservationId == invoice.CustomerReservationId);
            if (invoice.Type == "Final" && vehicleLog == null)
            {
                throw new DataNotFoundException("No vehicleLog found");
            }

            var bookingConfirmation = new BookingConfirmationDto
            {
                CustomerReservationId = customerReservation.Id,
                Make = customerReservation.Vehicle.VehicleModel.VehicleMake.Name,
                ModelName = customerReservation.Vehicle.VehicleModel.Name,
                StartDate = customerReservation.Reservation.StartDate,
                EndDate = customerReservation.Reservation.EndDate,
                StartTime = customerReservation.Reservation.StartTime,
                EndTime = customerReservation.Reservation.EndTime,
                Deposit = customerReservation.Vehicle.VehicleType.DepositAmount,
                ExtraKMCost = vehicleLog == null? 0 : vehicleLog.ExtraKM * customerReservation.Vehicle.CostPerExtraKM,
                Penalty = vehicleLog == null? 0 : vehicleLog.Penalty,
                RentalCost = vehicleLog == null? 0 : customerReservation.Vehicle.CostPerDay * customerReservation.Reservation.NoOfDays,
                Amount = invoice.Amount,
                Thumbnail = customerReservation.Vehicle.Thumbnail,
                InvoiceType = invoice.Type
            };

            return bookingConfirmation;
        }
    }
}
