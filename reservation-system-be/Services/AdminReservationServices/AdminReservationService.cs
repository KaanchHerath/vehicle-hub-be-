using Microsoft.AspNetCore.Http.HttpResults;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.EmployeeServices;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Services.AdminReservationServices
{
    public class AdminReservationService : IAdminReservationService
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;
        private readonly IEmployeeService _employeeService;
        private readonly IReservationService _reservationService;

        public AdminReservationService(DataContext context, ICustomerReservationService customerReservationService, IEmployeeService employeeService, IReservationService reservationService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _employeeService = employeeService;
            _reservationService = reservationService;
        }

        public async Task AcceptReservation(int id, int eid)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);
            var employee = await _employeeService.GetEmployee(eid);

            customerReservation.Reservation.Status = Status.Pending;
            customerReservation.Reservation.EmployeeId = employee.Id;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);
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
    }
}
