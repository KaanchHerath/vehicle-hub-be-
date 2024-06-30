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
            .Include(cr => cr.Reservation)
            .Include(cr => cr.Vehicle)
            .ThenInclude(v => v.VehicleType)
            .Include(cr => cr.Vehicle)
            .ThenInclude(v => v.VehicleModel)
            .ThenInclude(vm => vm.VehicleMake)
            .Include(cr => cr.Vehicle)
            .ThenInclude(v => v.Employee)
            .Select(cr => new CustomerReservationDto
            {
                Id = cr.Id,
                Customer = cr.Customer,
                Reservation = cr.Reservation,
                Vehicle = new VehicleDto
                {
                    Id = cr.Vehicle.Id,
                    RegistrationNumber = cr.Vehicle.RegistrationNumber,
                    ChassisNo = cr.Vehicle.ChassisNo,
                    Colour = cr.Vehicle.Colour,
                    Mileage = cr.Vehicle.Mileage,
                    CostPerDay = cr.Vehicle.CostPerDay,
                    CostPerExtraKM = cr.Vehicle.CostPerExtraKM,
                    Transmission = cr.Vehicle.Transmission,
                    Thumbnail = cr.Vehicle.Thumbnail,
                    FrontImg = cr.Vehicle.FrontImg,
                    RearImg = cr.Vehicle.RearImg,
                    DashboardImg = cr.Vehicle.DashboardImg,
                    InteriorImg = cr.Vehicle.InteriorImg,
                    Status = cr.Vehicle.Status,
                    Employee = cr.Vehicle.Employee,
                    VehicleType = cr.Vehicle.VehicleType,
                    VehicleModel = new VehicleModelMakeDto
                    {
                        Id = cr.Vehicle.VehicleModel.Id,
                        Name = cr.Vehicle.VehicleModel.Name,
                        Year = cr.Vehicle.VehicleModel.Year,
                        EngineCapacity = cr.Vehicle.VehicleModel.EngineCapacity,
                        SeatingCapacity = cr.Vehicle.VehicleModel.SeatingCapacity,
                        Fuel = cr.Vehicle.VehicleModel.Fuel,
                        VehicleMake = cr.Vehicle.VehicleModel.VehicleMake
                    },
                },
               
            })
            .ToListAsync();

            customerReservations = customerReservations.OrderByDescending(cr => cr.Reservation.StartDate).ToList();

            return customerReservations;
        }



        public async Task<CustomerReservationDto> GetCustomerReservation(int id)
        {
            var customerReservation = await _context.CustomerReservations
            .Include(cr => cr.Customer)
            .Include(cr => cr.Reservation)
            .Include(cr => cr.Vehicle)
            .ThenInclude(v => v.VehicleType)
            .Include(cr => cr.Vehicle)
            .ThenInclude(v => v.VehicleModel)
            .ThenInclude(vm => vm.VehicleMake)
            .Include(cr => cr.Vehicle)
            .ThenInclude(v => v.Employee)
            .Select(cr => new CustomerReservationDto
            {
                Id = cr.Id,
                Customer = cr.Customer,
                Reservation = cr.Reservation,
                Vehicle = new VehicleDto
                {
                    Id = cr.Vehicle.Id,
                    RegistrationNumber = cr.Vehicle.RegistrationNumber,
                    ChassisNo = cr.Vehicle.ChassisNo,
                    Colour = cr.Vehicle.Colour,
                    Mileage = cr.Vehicle.Mileage,
                    CostPerDay = cr.Vehicle.CostPerDay,
                    CostPerExtraKM = cr.Vehicle.CostPerExtraKM,
                    Transmission = cr.Vehicle.Transmission,
                    Thumbnail = cr.Vehicle.Thumbnail,
                    FrontImg = cr.Vehicle.FrontImg,
                    RearImg = cr.Vehicle.RearImg,
                    DashboardImg = cr.Vehicle.DashboardImg,
                    InteriorImg = cr.Vehicle.InteriorImg,
                    Status = cr.Vehicle.Status,
                    Employee = cr.Vehicle.Employee,
                    VehicleType = cr.Vehicle.VehicleType,
                    VehicleModel = new VehicleModelMakeDto
                    {
                        Id = cr.Vehicle.VehicleModel.Id,
                        Name = cr.Vehicle.VehicleModel.Name,
                        Year = cr.Vehicle.VehicleModel.Year,
                        EngineCapacity = cr.Vehicle.VehicleModel.EngineCapacity,
                        SeatingCapacity = cr.Vehicle.VehicleModel.SeatingCapacity,
                        Fuel = cr.Vehicle.VehicleModel.Fuel,
                        VehicleMake = cr.Vehicle.VehicleModel.VehicleMake
                    },
                },

            })
                .FirstOrDefaultAsync(cr => cr.Id == id);

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

        public async Task<CustomerReservation> UpdateCustomerReservation(int id, CreateCustomerReservationDto customerReservation)
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
