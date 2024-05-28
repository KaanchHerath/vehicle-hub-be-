using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.EmployeeServices;
using reservation_system_be.Services.VehicleModelServices;
using reservation_system_be.Services.VehicleTypeServices;

namespace reservation_system_be.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {
        private readonly DataContext _context;
        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IEmployeeService _employeeService;

        public VehicleService(DataContext context,IVehicleTypeService vehicleTypeService, IVehicleModelService vehicleModelService, IEmployeeService employeeService)
        {
            _context = context;
            _vehicleTypeService = vehicleTypeService;
            _vehicleModelService = vehicleModelService;
            _employeeService = employeeService;
        }
        public async Task<IEnumerable<VehicleDto>> GetAllVehicles()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .Include(v => v.Employee)
                .ToListAsync();

            if (vehicles == null || !vehicles.Any())
            {
                throw new DataNotFoundException("No vehicles found");
            }

            var vehicleDtos = new List<VehicleDto>();
            foreach (var vehicle in vehicles)
            {
                var vehicleType = await _vehicleTypeService.GetSingleVehicleType(vehicle.VehicleTypeId);
                var vehicleModel = await _vehicleModelService.GetVehicleModel(vehicle.VehicleModelId);
                var employee = await _employeeService.GetEmployee(vehicle.EmployeeId);
                
                var vehicleDto = new VehicleDto
                {
                    Id = vehicle.Id,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    ChassisNo = vehicle.ChassisNo,
                    Colour = vehicle.Colour,
                    Mileage = vehicle.Mileage,
                    CostPerDay = vehicle.CostPerDay,
                    Transmission = vehicle.Transmission,
                    CostPerExtraKM = vehicle.CostPerExtraKM,
                    Status = vehicle.Status,
                    VehicleType = vehicleType,
                    VehicleModel = vehicleModel,
                    Employee = employee
                };
                vehicleDtos.Add(vehicleDto);
            }
            return vehicleDtos;
        }

        public async Task<VehicleDto> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .Include(v => v.Employee)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }
            var vehicleDto = new VehicleDto
            {
                Id = vehicle.Id,
                RegistrationNumber = vehicle.RegistrationNumber,
                ChassisNo = vehicle.ChassisNo,
                Colour = vehicle.Colour,
                Mileage = vehicle.Mileage,
                CostPerDay = vehicle.CostPerDay,
                Transmission = vehicle.Transmission,
                CostPerExtraKM = vehicle.CostPerExtraKM,
                Status = vehicle.Status,
                VehicleType = await _vehicleTypeService.GetSingleVehicleType(vehicle.VehicleTypeId),
                VehicleModel = await _vehicleModelService.GetVehicleModel(vehicle.VehicleModelId),
                Employee = await _employeeService.GetEmployee(vehicle.EmployeeId)
            };
            return vehicleDto;
        }
        public async Task<Vehicle> CreateVehicle(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicle(int id, Vehicle vehicle)
        {
            var existingVehicle = _context.Vehicles.Find(id);
            if (existingVehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }
            existingVehicle.RegistrationNumber = vehicle.RegistrationNumber;
            existingVehicle.ChassisNo = vehicle.ChassisNo;
            existingVehicle.Colour = vehicle.Colour;
            existingVehicle.Mileage = vehicle.Mileage;
            existingVehicle.CostPerDay = vehicle.CostPerDay;
            existingVehicle.Transmission = vehicle.Transmission;
            existingVehicle.CostPerExtraKM = vehicle.CostPerExtraKM;
            existingVehicle.Status = vehicle.Status;
            existingVehicle.VehicleTypeId = vehicle.VehicleTypeId;
            existingVehicle.VehicleModelId = vehicle.VehicleModelId;
            existingVehicle.EmployeeId = vehicle.EmployeeId;
            _context.Entry(existingVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicle;

        }
        public async Task DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Vehicle>> SearchVehicle(string search)
        {
            var vehicles = await _context.Vehicles
                .Join(
                    _context.VehicleTypes,
                    vehicle => vehicle.VehicleTypeId,
                    vehicleType => vehicleType.Id,
                    (vehicle, vehicleType) => new { Vehicle = vehicle, VehicleType = vehicleType }
                )
                .Join(
                    _context.VehicleModels,
                    vehicleWithType => vehicleWithType.Vehicle.VehicleModelId,
                    vehicleModel => vehicleModel.Id,
                    (vehicleWithType, vehicleModel) => new { Vehicle = vehicleWithType.Vehicle, VehicleType = vehicleWithType.VehicleType, VehicleModel = vehicleModel }
                )
                .Where(
                    vehicleModelWithType => vehicleModelWithType.VehicleModel.Name.Contains(search) ||
                                             vehicleModelWithType.VehicleType.Name.Contains(search)
                )
                .Select(result => result.Vehicle)
                .ToListAsync();

            return vehicles;
        }


    }
}
