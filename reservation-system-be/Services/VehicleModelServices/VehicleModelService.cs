using reservation_system_be.Data;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;
using reservation_system_be.Services.AdditionalFeaturesServices;

namespace reservation_system_be.Services.VehicleModelServices
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly DataContext _context;
  
        public VehicleModelService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleModel>> GetAllVehicleModels()
        {
            return await _context.VehicleModels.ToListAsync();
        }

        public async Task<VehicleModel> GetVehicleModel(int id)
        {
            var vehicleModel = await _context.VehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                throw new DataNotFoundException("Vehicle model not found");
            }
            return vehicleModel;
        }

        public async Task<VehicleModel> CreateVehicleModel(VehicleModel vehicleModel)
        {
            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();
            return vehicleModel;
        }

        public async Task<VehicleModel> UpdateVehicleModel(int id, VehicleModel vehicleModel)
        {
            var existingVehicleModel = await _context.VehicleModels.FindAsync(id);
            if (existingVehicleModel == null)
            {
                throw new DataNotFoundException("Vehicle model not found");
            }
            existingVehicleModel.Name = vehicleModel.Name;
            existingVehicleModel.VehicleMakeId = vehicleModel.VehicleMakeId;
            existingVehicleModel.Year = vehicleModel.Year;
            existingVehicleModel.EngineCapacity = vehicleModel.EngineCapacity;
            existingVehicleModel.SeatingCapacity = vehicleModel.SeatingCapacity;
            existingVehicleModel.Fuel = vehicleModel.Fuel;
            existingVehicleModel.VehicleMakeId = vehicleModel.VehicleMakeId;
            _context.Entry(existingVehicleModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleModel;
        }

        public Task DeleteVehicleModel(int id)
        {
            var vehicleModel = _context.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                throw new DataNotFoundException("Vehicle model not found");
            }
            _context.VehicleModels.Remove(vehicleModel);
            return _context.SaveChangesAsync();
        }
    }
}
