using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Services.ReservationService;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.CustomerReservationService
{
    public class CustomerReservationService : ICustomerReservationService
    {
        private readonly DataContext _context;
        private readonly IReservationService _reservationService;
        private readonly ICustomerService _customerService;
        private readonly IVehicleService _vehicleService;

        public CustomerReservationService(DataContext context, IReservationService reservationService, ICustomerService customerService, IVehicleService vehicleService)
        {
            _context = context;
            _reservationService = reservationService;
            _customerService = customerService;
            _vehicleService = vehicleService;
        }

        public async Task<List<CustomerReservation>> GetAllCustomerReservations()
        {
            return await _context.CustomerReservations.ToListAsync();
        }

        public async Task<CustomerReservation> GetCustomerReservation(int id)
        {
            var customerReservation = await _context.CustomerReservations.FindAsync(id);
            if (customerReservation == null)
            {
                throw new DataNotFoundException("Customer Reservation not found");
            }
            return customerReservation;
        }

        public async Task<CustomerReservation> CreateCustomerReservation(CreateCustomerReservationDto customerReservationDto)
        {
            var reservation = await _reservationService.CreateReservation(customerReservationDto.Reservation);
            var customerReservation = new CustomerReservation
            {
                CustomerId = customerReservationDto.CustomerId,
                VehicleId = customerReservationDto.VehicleId,
                ReservationId = reservation.Id
            };
            _context.CustomerReservations.Add(customerReservation);
            await _context.SaveChangesAsync();
            return customerReservation;
        }

        public async Task<CustomerReservation> UpdateCustomerReservation(int id, CreateCustomerReservationDto customerReservationDto)
        {
            var existingCustomerReservation = await _context.CustomerReservations.FindAsync(id);
            if (existingCustomerReservation == null)
            {
                throw new DataNotFoundException("Customer Reservation not found");
            }
            existingCustomerReservation.VehicleId = customerReservationDto.VehicleId;
            _context.Entry(existingCustomerReservation.VehicleId).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingCustomerReservation;

        }

        public async Task DeleteCustomerReservation(int id)
        {
            var customerReservation = await _context.CustomerReservations.FindAsync(id);
            if (customerReservation == null)
            {
                throw new DataNotFoundException("Customer Reservation not found");
            }
            _context.CustomerReservations.Remove(customerReservation);
            await _context.SaveChangesAsync();
        }
    }
}
