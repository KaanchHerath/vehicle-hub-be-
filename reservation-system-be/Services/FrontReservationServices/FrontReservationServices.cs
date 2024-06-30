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
using reservation_system_be.Services.ReservationService;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.FrontReservationServices
{
    public class FrontReservationServices : IFrontReservationServices
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;
        private readonly IEmailService _emailService;
        private readonly ICustomerService _customerService;
        private readonly IInvoiceService _invoiceService;
        private readonly IVehicleService _vehicleService;
        private readonly IReservationService _reservationService;
        public FrontReservationServices(DataContext context, ICustomerReservationService customerReservationService, IEmailService emailService,
            ICustomerService customerService, IInvoiceService invoiceService, IVehicleService vehicleService, IReservationService reservationService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _emailService = emailService;
            _customerService = customerService;
            _invoiceService = invoiceService;
            _vehicleService = vehicleService;
            _reservationService = reservationService;
        }

        public async Task<CustomerReservation> RequestReservations(CreateCustomerReservationDto customerReservationDto)
        {
            var customerReservation = await _customerReservationService.CreateCustomerReservation(customerReservationDto);

            var cr = await _customerReservationService.GetCustomerReservation(customerReservation.Id);
            MailRequest mailRequest = new MailRequest
            {
                ToEmail = cr.Customer.Email,
                Subject = "VehicleHub - Reservation Request",
                Body = RequestMail(cr.Id, cr.Vehicle.VehicleModel.Name, cr.Vehicle.VehicleModel.VehicleMake.Name, cr.Reservation.StartDate, cr.Reservation.StartTime,cr.Reservation.EndDate, cr.Reservation.EndTime)
            };
            await _emailService.SendEmailAsync(mailRequest);

            return customerReservation;
        }

        private string RequestMail(int reservationId, string model, string make, DateTime startDate, TimeOnly startTime, DateTime endDate, TimeOnly endTime)
        {
            string formattedPickupDateTime = startDate.ToString("dddd, MMMM dd, yyyy") + " at " + startTime.ToString("hh:mm tt");
            string formattedDropoffDateTime = endDate.ToString("dddd, MMMM dd, yyyy") + " at " + endTime.ToString("hh:mm tt");
            string carName = make + " " + model;

            string response = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8' />
                    <title>Request Mail</title>
                </head>
                <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                    <div style='text-align: center; margin-bottom: 20px'>
                        <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                    </div>
                    <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; width: fit-content;'>
                        <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Request received</h1>
                        <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #" + reservationId + @"</h2>
                        <p style='color: #888888; text-align: center; font-size: 14px; margin-top: 5px;'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                        <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>We have received your request for the vehicle. Here are the details:</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0'><strong>Car Name:</strong> " + carName + @"</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0'><strong>Pickup Date & Time:</strong> " + formattedPickupDateTime + @"</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0'><strong>Dropoff Date & Time:</strong> " + formattedDropoffDateTime + @"</p>
                        <p style='color: #000000; text-align: left; margin-top: 20px;'>We appreciate your business and look forward to serving you.</p>
                        <p style='color: #000000; text-align: left; margin-bottom: 5px;'>Best regards,</p>
                        <p style='color: #000000; text-align: left; margin-top: 5px;'><strong>VehicleHub Team</strong></p>
                        <p style='padding: 10px; margin-top: 40px; text-align: center;'>Contact us: <a href='mailto:vehiclehub01@gmail.com'>vehiclehub01@gmail.com</a> | <a href='tel:+94771234567'>+94 77 123 4567</a></p>
                    </div>
                    <div style='text-align: center; margin-top: 20px; color: #7f7f7f;'>
                        <p style='font-size: 12px;'><strong>All rights reserved @VehicleHub. " + DateTime.Now.Year + @"</strong></p>
                        <p style='font-size: 12px;'>1234 Galle Road, Colombo, Sri Lanka</p>
                    </div>
                </body>
                </html>";
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
                VehicleId = customerReservation.Vehicle.Id,
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
                    throw new DataNotFoundException("No vehicle log found found");
                }
                extraKMCost = vehicleLog.ExtraKM * customerReservation.Vehicle.CostPerExtraKM;
                penalty = vehicleLog.Penalty;

                var invoice = await _context.Invoices.Where(i => i.Type == "Final").FirstOrDefaultAsync(i => i.CustomerReservationId == id);
                if (invoice == null)
                {
                    throw new DataNotFoundException("No final invoice found");
                }
                amount = invoice.Amount;
            }
            else if (customerReservation.Reservation.Status == Status.Cancelled)
            {
                var invoice = await _context.Invoices.Where(i => i.Type == "Deposit").FirstOrDefaultAsync(i => i.CustomerReservationId == id);
                if (invoice != null)
                {
                    amount = invoice.Amount;
                }
            }
            else
            {
                throw new DataNotFoundException("No rental history found");
            }

            var rentalHistorySingle = new RentalHistorySingleDto
            {
                CustomerReservationId = customerReservation.Id,
                VehicleId = customerReservation.Vehicle.Id,
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

        public async Task<DetailCarDto> GetVehicleDetails(int id)
        {
            var vehicle = await _vehicleService.GetVehicle(id);

            var detailCarDto = new DetailCarDto
            {
                Id = vehicle.Id,
                Make = vehicle.VehicleModel.VehicleMake.Name,
                Model = vehicle.VehicleModel.Name,
                Colour = vehicle.Colour,
                Mileage = vehicle.Mileage,
                CostPerDay = vehicle.CostPerDay,
                CostPerExtraKM = vehicle.CostPerExtraKM,
                Transmission = vehicle.Transmission,
                SeatingCapacity = vehicle.VehicleModel.SeatingCapacity,
                Year = vehicle.VehicleModel.Year,
                EngineCapacity = vehicle.VehicleModel.EngineCapacity,
                FuelType = vehicle.VehicleModel.Fuel
            };

            return detailCarDto;
        }

        public async Task CancelReservation(int id)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            customerReservation.Reservation.Status = Status.Cancelled;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);
        }
    }
}

