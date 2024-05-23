using Microsoft.AspNetCore.Http.HttpResults;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Services.AdminServices
{
    public class AdminService : IAdminService
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;
        private readonly IReservationService _reservationService;

        public AdminService(DataContext context, ICustomerReservationService customerReservationService, IReservationService reservationService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
            _reservationService = reservationService;
        }

        public async Task AcceptReservation(int id, Employee employee)
        {
            var customerReservation = await _customerReservationService.GetCustomerReservation(id);

            if (customerReservation == null)
            {
                throw new DataNotFoundException("Reservation not found");
            }

            customerReservation.Reservation.Status = Status.Pending;
            customerReservation.Reservation.EmployeeId = employee.Id;

            await _reservationService.UpdateReservation(customerReservation.Reservation.Id, customerReservation.Reservation);
        }
    }
}
