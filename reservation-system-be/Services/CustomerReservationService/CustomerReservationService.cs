using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.ReservationService;

namespace reservation_system_be.Services.CustomerReservationService
{
    public class CustomerReservationService : ICustomerReservationService
    {
        private readonly DataContext _context;
        private readonly IReservationService _reservationService;

        public CustomerReservationService(DataContext context, IReservationService reservationService)
        {
            _context = context;
            _reservationService = reservationService;
        }

        public async Task<List<CustomerReservation>> GetAllCustomerReservations()
        {
            return await _context.CustomerReservations
                .Include(cr => cr.Customer)
                .Include(cr => cr.Vehicle)
                .Include(cr => cr.Reservation)
                .ToListAsync();
        }

        public async Task<CustomerReservation> GetCustomerReservation(int id)
        {
            var customerReservation = await _context.CustomerReservations
                .Include(cr => cr.Customer)
                .Include(cr => cr.Vehicle)
                .Include(cr => cr.Reservation)
                .FirstOrDefaultAsync(cr => cr.Id == id);
            if (customerReservation == null)
            {
                throw new DataNotFoundException("Customer Reservation not found");
            }
            return customerReservation;
                
        }

        public async Task<CustomerReservation> CreateCustomerReservation(CustomerReservationDto customerReservationDto)
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

        public async Task<CustomerReservation> UpdateCustomerReservation(int id, CustomerReservationDto customerReservationDto)
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
