using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerServices;
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

        public CustomerReservationService(DataContext context, IReservationService reservationService)
        {
            _context = context;
            _reservationService = reservationService;
        }

        public async Task<IEnumerable<object>> GetAllCustomerReservations()
        {
            var customerReservations = await _context.CustomerReservations
                .Include(cr => cr.Customer)
                .Include(cr => cr.Vehicle)
                .Include(cr => cr.Reservation)
                .Select(cr => new
                {
                    Id = cr.Id,
                    Customer = new
                    {
                        Id = cr.Customer.Id,
                        Name = cr.Customer.Name,
                        NIC = cr.Customer.NIC,
                        DrivingLicenseNo = cr.Customer.DrivingLicenseNo,
                        Email = cr.Customer.Email,
                        Status = cr.Customer.Status,
                        ContactNo = cr.Customer.ContactNo,
                        Address = cr.Customer.Address
                    },
                    Vehicle = new
                    {
                        Id = cr.Vehicle.Id,
                        RegistrationNumber = cr.Vehicle.RegistrationNumber,
                        ChassisNo = cr.Vehicle.ChassisNo,
                        Colour = cr.Vehicle.Colour,
                        Mileage = cr.Vehicle.Mileage,
                        CostPerDay = cr.Vehicle.CostPerDay,
                        Transmission = cr.Vehicle.Transmission,
                        VehicleTypeId = cr.Vehicle.VehicleTypeId,
                        VehicleModelId = cr.Vehicle.VehicleModelId,
                        EmployeeId = cr.Vehicle.EmployeeId
                    },
                    Reservation = new
                    {
                        Id = cr.Reservation.Id,
                        StartTime = cr.Reservation.StartTime,
                        EndTime = cr.Reservation.EndTime,
                        StartDate = cr.Reservation.StartDate,
                        EndDate = cr.Reservation.EndDate,
                        EmployeeId = cr.Reservation.EmployeeId,
                        Status = cr.Reservation.Status
                    }
                })
                .ToListAsync();

            return customerReservations;
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
