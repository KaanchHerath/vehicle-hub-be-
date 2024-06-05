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
using reservation_system_be.Services.ReservationService;
using reservation_system_be.Services.VehicleLogServices;
using reservation_system_be.Services.VehicleServices;

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

        public AdminReservationService(DataContext context, ICustomerReservationService customerReservationService, IEmployeeService employeeService, 
            IReservationService reservationService, IInvoiceService invoiceService, IEmailService emailService, IVehicleLogService vehicleLogService,
                IVehicleService vehicleService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _employeeService = employeeService;
            _reservationService = reservationService;
            _invoiceService = invoiceService;
            _emailService = emailService;
            _vehicleLogService = vehicleLogService;
            _vehicleService = vehicleService;
        }

        public async Task AcceptReservation(int id, int eid)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);
            var employee = await _employeeService.GetEmployee(eid);

            customerReservation.Reservation.Status = Status.Pending;
            customerReservation.Reservation.EmployeeId = employee.Id;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);

            var invoice_model = new Invoice
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

        }

        private string DepositRequestMail(int id)
        {
            // Construct the payment link with the invoice ID
            string paymentLink = "https://yourdomain.com/payment/" + id; // fix link

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
            response += "<p>Contact us: info@vehiclehub.com | (123) 456-7890</p>";
            response += "<p>1234 Main St, Anytown, USA</p>";
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

        public async Task BeginReservation(int id)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            if (customerReservation.Reservation.Status != Status.Confirmed)
            {
                throw new Exception("Not a confirmed Reservation");
            }

            customerReservation.Reservation.Status = Status.Ongoing;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);
        }

        public async Task EndReservation(int id, VehicleLogDto vehicleLog)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            if (customerReservation.Reservation.Status == Status.Ongoing)
            {
                customerReservation.Reservation.Status = Status.Ended;
                await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);
            }
            else if (customerReservation.Reservation.Status != Status.Ended)
            {
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
                ExtraKM = KM < 0 ? 0 : ExtraKM,
                CustomerReservationId = customerReservation.Id
            };
            var vl = await _vehicleLogService.CreateVehicleLog(vehicleLog_model);

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == customerReservation.Vehicle.Id);
            if (vehicle != null)
            {
                vehicle.Mileage = vl.EndMileage;
                await _vehicleService.UpdateVehicle(vehicle.Id, vehicle);
            }

            // Create final invoice
            var invoice_model = new Invoice
            {
                Type = "Final",
                Amount = CalFinalAmount(customerReservation, vl),
                CustomerReservationId = customerReservation.Id
            };
            await _invoiceService.CreateInvoice(invoice_model);
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
    }
}
