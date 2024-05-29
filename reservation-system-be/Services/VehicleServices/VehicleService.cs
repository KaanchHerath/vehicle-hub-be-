using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.DTOs;
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

        public VehicleService(DataContext context, IVehicleTypeService vehicleTypeService, IVehicleModelService vehicleModelService, IEmployeeService employeeService)
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

        public async Task<List<VehicleResponse>> SearchVehicle(string search)
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake) // Ensure VehicleMake is included
                .Where(v =>
                    v.VehicleModel.Name.Contains(search) ||
                    v.VehicleType.Name.Contains(search)
                )
                .Select(v => new VehicleResponse
                {
                    Vehicle = v,
                    vehicleMake = v.VehicleModel.VehicleMake,
                    vehicleModel = v.VehicleModel
                })
                .ToListAsync();

            return vehicles;
        }


        public async Task<List<VehicleResponse>> GetAllVehiclesDetails()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake)
                .Select(v => new VehicleResponse
                {
                    Vehicle = v,
                    vehicleMake = v.VehicleModel.VehicleMake,
                    vehicleModel = v.VehicleModel
                })
                .ToListAsync();

            return vehicles;
        }


        public async Task<List<VehicleResponse>> FilterVehicles(int? vehicleTypeId, int? vehicleMakeId, int? seatingCapacity, float? depositAmount)
        {
            var query = _context.Vehicles
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake)
                .Include(v => v.VehicleType)
                .AsQueryable();

            if (vehicleTypeId.HasValue)
            {
                query = query.Where(v => v.VehicleTypeId == vehicleTypeId.Value);
            }

            if (vehicleMakeId.HasValue)
            {
                var joinedQuery = query.Join(
                    _context.VehicleModels,
                    vehicle => vehicle.VehicleModelId,
                    vehicleModel => vehicleModel.Id,
                    (vehicle, vehicleModel) => new { vehicle, vehicleModel }
                );

                if (vehicleMakeId.HasValue)
                {
                    joinedQuery = joinedQuery.Where(vm => vm.vehicleModel.VehicleMakeId == vehicleMakeId.Value);
                }

                if (!(seatingCapacity == 0))
                {
                    joinedQuery = joinedQuery.Where(vm => vm.vehicleModel.SeatingCapacity < seatingCapacity);
                }

                query = joinedQuery.Select(vm => vm.vehicle);
            }

            if (depositAmount.HasValue)
            {
                query = query.Where(v => v.VehicleType.DepositAmount < depositAmount.Value);
            }

            var result = await query
                .Select(v => new VehicleResponse
                {
                    Vehicle = v,
                    vehicleMake = v.VehicleModel.VehicleMake,
                    vehicleModel = v.VehicleModel
                })
                .ToListAsync();

            return result;
        }


    }

    public class VehicleResponse
    {
        public Vehicle Vehicle { get; set; }

        public VehicleMake vehicleMake { get; set; }

        public VehicleModel vehicleModel { get; set; }
    }

}
