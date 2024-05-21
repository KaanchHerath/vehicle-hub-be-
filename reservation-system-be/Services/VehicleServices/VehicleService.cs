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
