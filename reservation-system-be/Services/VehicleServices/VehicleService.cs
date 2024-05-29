using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {
        private readonly DataContext _context;

        public VehicleService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Vehicle>> GetAllVehicles()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }
            return vehicle;
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
