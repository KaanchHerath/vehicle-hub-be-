using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Helper;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerServices;
using reservation_system_be.Services.EmailServices;
using reservation_system_be.Services.ReservationService;
using reservation_system_be.Services.VehicleServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace reservation_system_be.Services.CustomerReservationService
{
    public class CustomerReservationService : ICustomerReservationService
    {
        private readonly DataContext _context;
        private readonly IReservationService _reservationService;
        private readonly ICustomerService _customerService;
        private readonly IVehicleService _vehicleService;
        private readonly IEmailService _emailService;

        public CustomerReservationService(DataContext context, IReservationService reservationService, ICustomerService customerService, IVehicleService vehicleService,IEmailService emailService)
        {
            _context = context;
            _reservationService = reservationService;
            _customerService = customerService;
            _vehicleService = vehicleService;
            _emailService = emailService;
        }

        public async Task<IEnumerable<CustomerReservationDto>> GetAllCustomerReservations()
        {
            var customerReservations = await _context.CustomerReservations
            .Include(cr => cr.Customer)
            .Include(cr => cr.Vehicle)
            .Include(cr => cr.Reservation)
            .ToListAsync();

            if (customerReservations == null || !customerReservations.Any())
            {
                throw new DataNotFoundException("No customer reservations found");
            }

            var customerReservationDtos = new List<CustomerReservationDto>();

            foreach (var customerReservation in customerReservations)
            {
                var customerDto = await _customerService.GetCustomer(customerReservation.CustomerId);
                var vehicleDto = await _vehicleService.GetVehicle(customerReservation.VehicleId);
                var reservationDto = await _reservationService.GetReservation(customerReservation.ReservationId);

                var customerReservationDto = new CustomerReservationDto
                {
                    Id = customerReservation.Id,
                    Customer = customerDto,
                    Vehicle = vehicleDto,
                    Reservation = reservationDto
                };

                customerReservationDtos.Add(customerReservationDto);
            }

            customerReservationDtos = customerReservationDtos.OrderByDescending(cr => cr.Reservation.StartDate).ToList();

            return customerReservationDtos;
        }



        public async Task<CustomerReservationDto> GetCustomerReservation(int id)
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

            var customerReservationDto = new CustomerReservationDto
            {
                Id = customerReservation.Id,
                Customer = await _customerService.GetCustomer(customerReservation.CustomerId),
                Vehicle = await _vehicleService.GetVehicle(customerReservation.VehicleId),
                Reservation = await _reservationService.GetReservation(customerReservation.ReservationId)
            };

            return customerReservationDto;
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

        public async Task<CustomerReservation> UpdateCustomerReservation(int id, CustomerReservation customerReservation)
        {
            var existingCustomerReservation = await _context.CustomerReservations.FindAsync(id);
            if (existingCustomerReservation == null)
            {
                throw new DataNotFoundException("Customer Reservation not found");
            }
            existingCustomerReservation.VehicleId = customerReservation.VehicleId;
            _context.Entry(existingCustomerReservation).State = EntityState.Modified;
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
