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
            string paymentLink = "http://localhost:3000/bookingconfirmation/" + encryptedId;

            string response = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8' />
                    <title>Deposit Request Mail</title>
                </head>
                <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                    <div style='text-align: center; margin-bottom: 20px'>
                        <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                    </div>
                    <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 650px;'>
                        <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Reservation accepted!</h1>
                        <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #"" + reservationId + @""</h2>
                        <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px;'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                        <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>Thank you for your vehicle reservation request.</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0;'>To confirm your reservation, please make a deposit payment within the next 3 days. If we do not receive your payment, your pending reservation request will be cancelled.</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0;'>You can make your payment by clicking the link below:</p>
                        <p style='text-align: center; margin: 50px 0;'>
                            <a href='" + paymentLink + @"' style='background-color: #283280; border: none; color: #fbdac6; padding: 15px 20px; text-decoration: none; border-radius: 5px; font-size: small;'>Make Deposit Payment</a>
                        </p>
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

            string response = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8' />
                    <title>Reservation Declined Mail</title>
                </head>
                <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                    <div style='text-align: center; margin-bottom: 20px'>
                        <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                    </div>
                    <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 650px;'>
                        <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Reservation Request Declined</h1>
                        <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px;'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                        <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>Dear Customer,</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0;'>We regret to inform you that your vehicle reservation request with ID #" + id + @" has been declined.</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0;'>We apologize for any inconvenience this may have caused. If you have any questions or need further details, please feel free to contact us.</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0;'>Our team is here to assist you and we hope to serve you in the future.</p>
                        <p style='color: #000000; text-align: left; margin: 5px 0;'>You can make another reservation by clicking the link below:</p>
                        <p style='text-align: center; margin: 50px 0;'>
                            <a href='" + vehicleFleetLink + @"' style='background-color: #283280; border: none; color: #fbdac6; padding: 15px 20px; text-decoration: none; border-radius: 5px; font-size: small;'>Make Another Reservation</a>
                        </p>
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
                Description = $"Your reservation with ID #{customerReservation.Id} has ended. Please make the final payment to the complete the reservation. Thank you for choosing VehicleHub!",
                Generated_DateTime = DateTime.Now,
                CustomerReservationId = customerReservation.Id
            };
            await _notificationService.AddNotification(notification);

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerReservation.Customer.Email,
                Subject = "Reservation Ended",
                Body = ReservationEndedMail(invoice.Id, customerReservation.Id)
            };
            await _emailService.SendEmailAsync(mailRequest);
        }

        private string ReservationEndedMail(int id, int reservationId)
        {
            string encryptedId = EncryptionHelper.Encrypt(id);
            string paymentLink = "http://localhost:3000/bookingconfirmation/" + encryptedId;

            string response = @"
                <!DOCTYPE html>
                <html>
                  <head>
                    <meta charset='UTF-8' />
                    <title>Final Payment Request Mail</title>
                  </head>
                  <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                    <div style='text-align: center; margin-bottom: 20px'>
                      <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;'/>
                    </div>
                    <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 650px;'>
                      <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Reservation Ended</h1>
                      <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #" + reservationId + @"</h2>
                      <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px;'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                      <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>Thank you for choosing VehicleHub for your vehicle reservation.</p>
                      <p style='color: #000000; text-align: left; margin: 5px 0;'>We hope you had a great experience with us. To complete your reservation, please make the final payment at your earliest convenience.</p>
                      <p style='color: #000000; text-align: left; margin: 5px 0;'>You can make your final payment by clicking the link below:</p>
                      <p style='text-align: center; margin: 50px 0;'><a href='" + paymentLink + @"'style='background-color: #283280; border: none; color: #fbdac6; padding: 15px 20px; text-decoration: none; border-radius: 5px; font-size: small;'>Make Final Payment</a></p>
                      <p style='color: #000000; text-align: left; margin-top: 20px;'>We appreciate your business and look forward to serving you again.</p>
                      <p style='color: #000000; text-align: left; margin-bottom: 5px;'>Best regards,</p>
                      <p style='color: #000000; text-align: left; margin-top: 5px;'><strong>VehicleHub Team</strong></p>
                      <p style='padding: 10px; margin-top: 40px; text-align: center;'>Contact us: <a href='mailto:vehiclehub01@gmail.com'>vehiclehub01@gmail.com</a> | <a href='tel:+94771234567'>+94 77 123 4567</a></p>
                    </div>
                    <div style='text-align: center; margin-top: 20px; color: #7f7f7f;'>
                      <p style='font-size: 12px;'><strong>All rights reserved @VehicleHub. "" + DateTime.Now.Year + @""</strong></p>
                      <p style='font-size: 12px;'>1234 Galle Road, Colombo, Sri Lanka</p>
                    </div>
                  </body>
                </html>
                ";

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

            var cr = await _customerReservationService.GetCustomerReservation(id);

            var notification = new Notification
            {
                Type = "Reservation",
                Title = "Vehicle Changed",
                Description = $"Your vehicle of Reservation #{customerReservation.Id} has been changed to {cr.Vehicle.VehicleModel.VehicleMake.Name} {cr.Vehicle.VehicleModel.Name}.",
                Generated_DateTime = DateTime.Now,
                CustomerReservationId = customerReservation.Id
            };
            await _notificationService.AddNotification(notification);

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerReservation.Customer.Email,
                Subject = "Vehicle Changed",
                Body = VehicleChangedMail(customerReservation.Id, cr.Vehicle.VehicleModel.VehicleMake.Name, cr.Vehicle.VehicleModel.Name, cr.Vehicle.Id, cr.Vehicle.VehicleModel.Year)
            };
            await _emailService.SendEmailAsync(mailRequest);
        }

        private string VehicleChangedMail(int reservationId, string make, string model, int vehicleId, int year)
        {
            string vehicleLink = "http://localhost:3000/vehiclefleet/" + vehicleId;

            string response = @"
                <!DOCTYPE html>
                <html><head><meta charset='UTF-8' /><title>Vehicle Change Notification</title></head>
                <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                  <div style='text-align: center; margin-bottom: 20px'>
                    <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                  </div>
                  <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 500px;'>
                    <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Vehicle Change Notification</h1>
                    <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #" + reservationId + @"</h2>
                    <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px;'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                    <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>We wanted to inform you that the vehicle for your reservation has been changed. The new vehicle details are as follows:</p>
                    <p style='color: #000000; text-align: left; margin: 5px 0;'><strong>Make:</strong> " + make + @"</p>
                    <p style='color: #000000; text-align: left; margin: 5px 0;'><strong>Model:</strong> " + model + @"</p>
                    <p style='color: #000000; text-align: left; margin: 5px 0; padding-bottom: 10px'><strong>Year:</strong> " + year + @"</p>
                    <p style='color: #000000; text-align: left; margin: 5px 0;'>If you are not satisfied with the changed vehicle, please notify us as soon as possible. You can contact us to request a cancellation and a refund of your deposit payment if it has already been made.</p>
                    <p style='text-align: center; margin: 50px 0;'><a href='" + vehicleLink + @"' style='background-color: #283280; border: none; color: #fbdac6; padding: 15px 20px; text-decoration: none; border-radius: 5px; font-size: small;'>View Vehicle</a></p>
                    <p style='color: #000000; text-align: left; margin-top: 20px;'>We apologize for any inconvenience caused and appreciate your understanding.</p>
                    <p style='color: #000000; text-align: left; margin-bottom: 5px;'>Best regards,</p>
                    <p style='color: #000000; text-align: left; margin-top: 5px;'><strong>VehicleHub Team</strong></p>
                    <p style='padding: 10px; margin-top: 40px; text-align: center;'>Contact us: <a href='mailto:vehiclehub01@gmail.com'>vehiclehub01@gmail.com</a> | <a href='tel:+94771234567'>+94 77 123 4567</a></p>
                  </div>
                  <div style='text-align: center; margin-top: 20px; color: #7f7f7f;'>
                    <p style='font-size: 12px;'><strong>All rights reserved @VehicleHub. " + DateTime.Now.Year + @"</strong></p>
                    <p style='font-size: 12px;'>1234 Galle Road, Colombo, Sri Lanka</p>
                  </div>
                </body></html>
                ";

            return response;
        }




        public async Task CancelReservation(int id, int eid)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            customerReservation.Reservation.Status = Status.Cancelled;
            customerReservation.Reservation.EmployeeId = eid;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);

            var notification = new Notification
            {
                Type = "Reservation",
                Title = "Reservation Cancelled",
                Description = "Your reservation has been cancelled. Please contact us for further details.",
                Generated_DateTime = DateTime.Now,
                CustomerReservationId = customerReservation.Id
            };
            await _notificationService.AddNotification(notification);

            MailRequest mailRequest = new MailRequest
            {
                ToEmail = customerReservation.Customer.Email,
                Subject = "Reservation Cancelled",
                Body = CancelMailBody(customerReservation.Id)
            };
            await _emailService.SendEmailAsync(mailRequest);
        }

        private string CancelMailBody(int reservationId)
        {
            string contactLink = "http://localhost:3000/contactus";

            string response = @"
                <!DOCTYPE html>
                <html><head><meta charset='UTF-8' /><title>Reservation Cancellation Notification</title></head>
                <body style='width: 100%; background-color: #f4f4f4; text-align: center; padding: 20px; font-family: Arial, sans-serif;'>
                  <div style='text-align: center; margin-bottom: 20px'>
                    <img src='https://drive.google.com/uc?export=view&id=1wlXifh_GzGGiA43mOQ_MX06LJ6soPqXM' alt='Vehicle Hub Logo' style='width: 200px; height: auto; display: inline-block; vertical-align: middle;' />
                  </div>
                  <div style='background-color: #ffffff; padding: 50px 50px 10px 50px; border-radius: 10px; margin: 20px auto; max-width: 650px;'>
                    <h1 style='color: #000000; margin: 20px 0; text-align: center; font-size: 40px;'>Reservation Cancellation</h1>
                    <h2 style='color: #000000; text-align: center; font-size: 18px; font-weight: normal; margin-bottom: 5px;'>Reservation #" + reservationId + @"</h2>
                    <p style='color: #888888; text-align: center; font-size: 14px; margin: 5px;'>" + DateTime.Now.ToString("MMM dd, yyyy") + @"</p>
                    <p style='color: #000000; text-align: left; padding-top: 40px; padding-bottom: 10px;'>We regret to inform you that due to unforeseen circumstances, we have had to cancel your vehicle reservation. We understand that this news is disappointing and we sincerely apologize for any inconvenience this may cause.</p>
                    <p style='color: #000000; text-align: left; margin: 5px 0;'>Please be assured that any deposit payment you have made will be refunded in full. Our team is working diligently to process the refund and you should receive it within the next 3-5 business days.</p>
                    <p style='color: #000000; text-align: left; margin: 5px 0;'>We truly value your business and appreciate your understanding in this matter. If you have any questions or need further assistance, please do not hesitate to contact us.</p>
                    <div style='text-align: center; margin: 40px auto;'>
                      <a href='" + contactLink + @"' style='background-color: #283280; border: none; color: #fbdac6; padding: 15px 20px; text-decoration: none; border-radius: 5px; font-size: small;'>Contact Us</a>
                    </div>
                    <p style='color: #000000; text-align: left; margin-top: 20px;'>Once again, we apologize for any inconvenience and thank you for your understanding.</p>
                    <p style='color: #000000; text-align: left; margin-bottom: 5px;'>Best regards,</p>
                    <p style='color: #000000; text-align: left; margin-top: 5px;'><strong>VehicleHub Team</strong></p>
                    <p style='padding: 10px; margin-top: 40px; text-align: center;'>Contact us: <a href='mailto:vehiclehub01@gmail.com'>vehiclehub01@gmail.com</a> | <a href='tel:+94771234567'>+94 77 123 4567</a></p>
                  </div>
                  <div style='text-align: center; margin-top: 20px; color: #7f7f7f;'>
                    <p style='font-size: 12px;'><strong>All rights reserved @VehicleHub. " + DateTime.Now.Year + @"</strong></p>
                    <p style='font-size: 12px;'>1234 Galle Road, Colombo, Sri Lanka</p>
                  </div>
                </body></html>
                ";

            return response;
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
