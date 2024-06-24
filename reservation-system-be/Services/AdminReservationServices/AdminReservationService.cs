using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.EmployeeServices;
using reservation_system_be.Services.InvoiceService;
using reservation_system_be.Services.NotificationServices;
using reservation_system_be.Services.ReservationService;
using reservation_system_be.Services.VehicleLogServices;
using reservation_system_be.Services.VehicleServices;
using Stripe;

namespace reservation_system_be.Services.AdminReservationServices
{
    public class AdminReservationService : IAdminReservationService
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;
        private readonly IEmployeeService _employeeService;
        private readonly IReservationService _reservationService;
        private readonly IInvoiceService _invoiceService;
        private readonly IEmailService _emailService;
        private readonly IVehicleLogService _vehicleLogService;
        private readonly IVehicleService _vehicleService;
        private readonly INotificationService _notificationService;

        public AdminReservationService(DataContext context, ICustomerReservationService customerReservationService, IEmployeeService employeeService, 
            IReservationService reservationService, IInvoiceService invoiceService, IEmailService emailService, IVehicleLogService vehicleLogService,
                IVehicleService vehicleService, INotificationService notificationService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _employeeService = employeeService;
            _reservationService = reservationService;
            _invoiceService = invoiceService;
            _emailService = emailService;
            _vehicleLogService = vehicleLogService;
            _vehicleService = vehicleService;
            _notificationService = notificationService;
        }

        public async Task AcceptReservation(int id, int eid)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);
            var employee = await _employeeService.GetEmployee(eid);

            customerReservation.Reservation.Status = Status.Pending;
            customerReservation.Reservation.EmployeeId = employee.Id;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);

            var invoice_model = new Models.Invoice
            {
                Type = "Deposit",
                Amount = customerReservation.Vehicle.VehicleType.DepositAmount,
                CustomerReservationId = customerReservation.Id
            };
            var invoice = await _invoiceService.CreateInvoice(invoice_model);

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerReservation.Customer.Email,
                Subject = "Reservation Accepted",
                Body = DepositRequestMail(invoice.Id)
            };
            await _emailService.SendEmailAsync(mailRequest);

            var notification = new Notification
            {
                Type = "Reservation",
                Title = "Reservation Accepted",
                Description = "Your reservation request has been accepted. Please make a deposit payment to confirm your reservation.",
                Generated_DateTime = DateTime.Now,
                CustomerReservationId = customerReservation.Id
            };
            await _notificationService.AddNotification(notification);

        }

        private string DepositRequestMail(int id)
        {
            string encryptedId = EncryptionHelper.Encrypt(id);
            // Construct the payment link with the invoice ID
            string paymentLink = "http://localhost:3000/bookingconfirmation/" + encryptedId; // fix link

            string response = "<div style=\"width:100%;background-color:#f4f4f4;text-align:center;margin:10px;padding:10px;font-family:Arial, sans-serif;\">";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;\">";
            response += "<h1>VehicleHub</h1>";
            response += "</div>";
            response += "<div style=\"margin:20px;text-align:left;\">";
            response += "<img src=\"https://drive.google.com/uc?export=view&id=1S40qYUDb_f9YRAaQeQmPETz5ABYbI32p\" alt=\"Company Logo\" style=\"width:150px;height:auto;display:block;margin:auto;\"/>";
            response += "<h2 style=\"text-align:center;\">Deposit Payment Required</h2>";
            response += "<p>Thank you for your vehicle reservation request.</p>";
            response += "<p style=\"margin-top:20px;\">To confirm your reservation, please make a deposit payment within the next 3 days. If we do not receive your payment, your pending reservation request will be cancelled.</p>";
            response += "<p>You can make your payment by clicking the link below:</p>";
            response += "<p style=\"text-align:center;\"><a href=\"" + paymentLink + "\" style=\"background-color:#283280;color:#ffffff;padding:10px 20px;text-decoration:none;border-radius:5px;\">Make Deposit Payment</a></p>";
            response += "<p>We appreciate your business and look forward to serving you.</p>";
            response += "<p>Best regards,</p>";
            response += "<p><strong>VehicleHub Team</strong></p>";
            response += "</div>";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;margin-top:20px;text-align:center;\">";
            response += "<p>Contact us: vehiclehub01@gmail.com | +94 77 123 4567</p>";
            response += "<p>1234 Galle Road, Colombo, Sri Lanka</p>";
            response += "</div>";
            response += "</div>";
            return response;
        }


        public async Task DeclineReservation(int id, int eid)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);
            var employee = await _employeeService.GetEmployee(eid);

            customerReservation.Reservation.Status = Status.Cancelled;
            customerReservation.Reservation.EmployeeId = employee.Id;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerReservation.Customer.Email,
                Subject = "Reservation Declined",
                Body = ReservationDeclinedMail(id)
            };
            await _emailService.SendEmailAsync(mailRequest);

            var notification = new Notification
            {
                Type = "Reservation",
                Title = "Reservation Declined",
                Description = "Your reservation request has been declined. Please contact us for further details.",
                Generated_DateTime = DateTime.Now,
                CustomerReservationId = customerReservation.Id
            };
            await _notificationService.AddNotification(notification);
        }

        private string ReservationDeclinedMail(int id)
        {
            // Construct the home link for redirection
            string vehicleFleetLink = "http://localhost:3000/vehiclefleet"; // fix link

            string response = "<div style=\"width:100%;background-color:#f4f4f4;text-align:center;margin:10px;padding:10px;font-family:Arial, sans-serif;\">";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;\">";
            response += "<h1>VehicleHub</h1>";
            response += "</div>";
            response += "<div style=\"margin:20px;text-align:left;\">";
            response += "<img src=\"https://drive.google.com/uc?export=view&id=1S40qYUDb_f9YRAaQeQmPETz5ABYbI32p\" alt=\"Company Logo\" style=\"width:150px;height:auto;display:block;margin:auto;\"/>";
            response += "<h2 style=\"text-align:center;\">Reservation Request Declined</h2>";
            response += "<p>Dear Customer,</p>";
            response += "<p>We regret to inform you that your vehicle reservation request with ID #" + id + " has been declined.</p>";
            response += "<p>We apologize for any inconvenience this may have caused. If you have any questions or need further details, please feel free to contact us.</p>";
            response += "<p>Our team is here to assist you and we hope to serve you in the future.</p>";
            response += "<p>You can make another reservation by clicking the link below:</p>";
            response += "<p style=\"text-align:center;\"><a href=\"" + vehicleFleetLink + "\" style=\"background-color:#283280;color:#ffffff;padding:10px 20px;text-decoration:none;border-radius:5px;\">Make Another Reservation</a></p>";
            response += "<p>Best regards,</p>";
            response += "<p><strong>VehicleHub Team</strong></p>";
            response += "</div>";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;margin-top:20px;text-align:center;\">";
            response += "<p>Contact us: vehiclehub01@gmail.com | +94 77 123 4567</p>";
            response += "<p>1234 Galle Road, Colombo, Sri Lanka</p>";
            response += "</div>";
            response += "</div>";
            return response;
        }


        public async Task<IEnumerable<ViewReservationDto>> ViewReservations()
        {
            var customerReservations = await _customerReservationService.GetAllCustomerReservations();
            var viewReservations = new List<ViewReservationDto>();

            foreach (var customerReservation in customerReservations)
            {
                var viewReservation = new ViewReservationDto
                {
                    Id = customerReservation.Id,
                    Name = customerReservation.Customer.Name,
                    Email = customerReservation.Customer.Email,
                    Phone = customerReservation.Customer.ContactNo,
                    RegNo = customerReservation.Vehicle.RegistrationNumber,
                    StartDate = customerReservation.Reservation.StartDate,
                    EndDate = customerReservation.Reservation.EndDate,
                    StartTime = customerReservation.Reservation.StartTime,
                    EndTime = customerReservation.Reservation.EndTime,
                    Status = customerReservation.Reservation.Status
                };

                viewReservations.Add(viewReservation);
            }

            return viewReservations;
        }

        public async Task BeginReservation(int id, int eid)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            if (customerReservation.Reservation.Status != Status.Confirmed)
            {
                throw new Exception("Not a confirmed Reservation");
            }

            customerReservation.Reservation.Status = Status.Ongoing;
            customerReservation.Reservation.EmployeeId = eid;
            _context.Entry(customerReservation.Reservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            var notification = new Notification
            {
                Type = "Reservation",
                Title = "Reservation Started",
                Description = "Your reservation has started. Enjoy your ride!",
                Generated_DateTime = DateTime.Now,
                CustomerReservationId = customerReservation.Id
            };
            await _notificationService.AddNotification(notification);
        }

        public async Task EndReservation(int id, int eid, VehicleLogDto vehicleLog)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            if (customerReservation.Reservation.Status == Status.Ongoing)
            {
                customerReservation.Reservation.EmployeeId = eid;
                customerReservation.Reservation.Status = Status.Ended;
                await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);
            }
            else if (customerReservation.Reservation.Status == Status.Ended)
            {
                customerReservation.Reservation.EmployeeId = eid;
                await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);

                var existingVehicleLog = await _context.VehicleLogs.FirstOrDefaultAsync(vl => vl.CustomerReservationId == id);
                if (existingVehicleLog != null)
                {
                    await _vehicleLogService.DeleteVehicleLog(existingVehicleLog.Id);
                }

                var existingInvoice = await _context.Invoices.FirstOrDefaultAsync(i => i.CustomerReservationId == id);
                if (existingInvoice != null)
                {
                    await _invoiceService.DeleteInvoice(existingInvoice.Id);
                }
            }
            else
            {
                throw new Exception("Reservation is in " + customerReservation.Reservation.Status + "status");
            }
            

            // Calculate KM & Extra KM
            var KM = vehicleLog.EndMileage - customerReservation.Vehicle.Mileage;
            var ExtraKM = KM - (customerReservation.Reservation.NoOfDays * 100);
            // Create vehicle log
            var vehicleLog_model = new VehicleLog
            {
                EndMileage = vehicleLog.EndMileage,
                Penalty = vehicleLog.Penalty,
                Description = vehicleLog.Description,
                ExtraKM = ExtraKM < 0 ? 0 : ExtraKM,
                CustomerReservationId = customerReservation.Id
            };
            var vl = await _vehicleLogService.CreateVehicleLog(vehicleLog_model);

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == customerReservation.Vehicle.Id);
            if (vehicle != null)
            {
                vehicle.Mileage = vl.EndMileage;
                _context.Entry(vehicle).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            // Create final invoice
            var invoice_model = new Models.Invoice
            {
                Type = "Final",
                Amount = CalFinalAmount(customerReservation, vl),
                CustomerReservationId = customerReservation.Id
            };
            var invoice = await _invoiceService.CreateInvoice(invoice_model);

            var notification = new Notification
            {
                Type = "Reservation",
                Title = "Reservation Ended",
                Description = "Your reservation has ended. Please make the final payment to the complete the reservation. Thank you for choosing VehicleHub!",
                Generated_DateTime = DateTime.Now,
                CustomerReservationId = customerReservation.Id
            };
            await _notificationService.AddNotification(notification);

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerReservation.Customer.Email,
                Subject = "Reservation Ended",
                Body = ReservationEndedMail(invoice.Id)
            };
            await _emailService.SendEmailAsync(mailRequest);
        }

        private string ReservationEndedMail(int id)
        {
            string encryptedId = EncryptionHelper.Encrypt(id);
            // Construct the home link for redirection
            string paymentLink = "http://localhost:3000/bookingconfirmation/" + encryptedId; // fix link

            string response = "<div style=\"width:100%;background-color:#f4f4f4;text-align:center;margin:10px;padding:10px;font-family:Arial, sans-serif;\">";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;\">";
            response += "<h1>VehicleHub</h1>";
            response += "</div>";
            response += "<div style=\"margin:20px;text-align:left;\">";
            response += "<img src=\"https://drive.google.com/uc?export=view&id=1S40qYUDb_f9YRAaQeQmPETz5ABYbI32p\" alt=\"Company Logo\" style=\"width:150px;height:auto;display:block;margin:auto;\"/>";
            response += "<h2 style=\"text-align:center;\">Reservation Ended</h2>";
            response += "<p>Dear Customer,</p>";
            response += "<p>Your reservation with ID #" + id + " has ended. We hope you enjoyed your ride with us.</p>";
            response += "<p>Please make the final payment to complete the reservation. You can make your payment by clicking the link below:</p>";
            response += "<p style=\"text-align:center;\"><a href=\"" + paymentLink + "\" style=\"background-color:#283280;color:#ffffff;padding:10px 20px;text-decoration:none;border-radius:5px;\">Make Final Payment</a></p>";
            response += "<p>Thank you for choosing VehicleHub. We look forward to serving you again.</p>";
            response += "<p>Best regards,</p>";
            response += "<p><strong>VehicleHub Team</strong></p>";
            response += "</div>";
            response += "<div style=\"background-color:#283280;color:#ffffff;padding:10px;margin-top:20px;text-align:center;\">";
            response += "<p>Contact us: vehiclehub01@gmail.com | +94 77 123 4567</p>";
            response += "<p>1234 Galle Road, Colombo, Sri Lanka</p>";
            response += "</div>";
            response += "</div>";
            return response;
        }

        private float CalFinalAmount(CustomerReservationDto customerReservation, VehicleLog vehicleLog)
        {
            var finalAmount = 0.0f;
            finalAmount+= customerReservation.Vehicle.CostPerDay * customerReservation.Reservation.NoOfDays; // Rental cost
            finalAmount+= vehicleLog.Penalty; // Penalty
            finalAmount+= vehicleLog.ExtraKM * customerReservation.Vehicle.CostPerExtraKM; //Extra KM cost
            finalAmount-= customerReservation.Vehicle.VehicleType.DepositAmount; // Deducting Deposit

            return finalAmount;
        }

        public async Task <IEnumerable<VehicleCardDto>> AvailableVehicles(int id) // customerReservationId
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            var reservation = customerReservation.Reservation;

            // Get the list of vehicles that are not reserved during the given period
            var reservedVehicleIds = await _context.CustomerReservations
                .Where(cr => cr.Reservation.StartDate <= reservation.EndDate && cr.Reservation.EndDate >= reservation.StartDate && (cr.Reservation.Status != Status.Cancelled || cr.Reservation.Status != Status.Completed))
                .Select(cr => cr.VehicleId)
                .ToListAsync();

            var availableVehicles = await _context.Vehicles
                .Where(v => !reservedVehicleIds.Contains(v.Id) && v.Status && v.VehicleTypeId == customerReservation.Vehicle.VehicleType.Id)
                .ToListAsync();

            var vehicleCardDtos = new List<VehicleCardDto>();

            foreach (var vehicle in availableVehicles)
            {
                var vehicleDto = await _vehicleService.GetVehicle(vehicle.Id);
                var vehicleCardDto = new VehicleCardDto
                {
                    Id = vehicleDto.Id,
                    Name = vehicleDto.VehicleModel.Name,
                    Make = vehicleDto.VehicleModel.VehicleMake.Name,
                    Type = vehicleDto.VehicleType.Name,
                    Year = vehicleDto.VehicleModel.Year,
                    Transmission = vehicleDto.Transmission,
                    SeatingCapacity = vehicleDto.VehicleModel.SeatingCapacity,
                    CostPerDay = vehicleDto.CostPerDay,
                    Thumbnail = vehicleDto.Thumbnail
                };
                vehicleCardDtos.Add(vehicleCardDto);
            }

            return vehicleCardDtos;
        }

        public async Task ReservationChangeVehicle(int id, int vid) // customerReservationId, vehicleId
        {
            var customerReservation = await _context.CustomerReservations.FirstOrDefaultAsync(cr => cr.Id == id);
            if (customerReservation == null)
            {
                throw new DataNotFoundException("Customer Reservation not found");
            }

            customerReservation.VehicleId = vid;

            _context.Entry(customerReservation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CancelReservation(int id, int eid)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            customerReservation.Reservation.Status = Status.Cancelled;
            customerReservation.Reservation.EmployeeId = eid;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);
        }

        public async Task<CustomerHoverDto> CustomerDetails(int id)
        {
            var customerReservation = await _context.CustomerReservations
                .Include(cr => cr.Customer)
                .FirstOrDefaultAsync(cr => cr.Id == id);

            var customerHoverDto = new CustomerHoverDto
            {
                Id = customerReservation.Customer.Id,
                Name = customerReservation.Customer.Name,
                Email = customerReservation.Customer.Email,
                Phone = customerReservation.Customer.ContactNo
            };

            return customerHoverDto;
        }


    }
}
