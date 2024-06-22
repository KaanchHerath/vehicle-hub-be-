using reservation_system_be.Data;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;
using reservation_system_be.Services.VehicleMakeServices;

namespace reservation_system_be.Services.VehicleModelServices
{
    public class VehicleModelService : IVehicleModelService
    {
        private readonly DataContext _context;
        private readonly IVehicleMakeService _vehicleMakeService;
   
  
        public VehicleModelService(DataContext context, IVehicleMakeService vehicleMakeService)
        {
            _context = context;
            _vehicleMakeService = vehicleMakeService;
        }

        public async Task<IEnumerable<VehicleModelMakeDto>> GetAllVehicleModels()
        {
            var vehicleModels = await _context.VehicleModels
            .Include(v => v.VehicleMake)
            .Select(v => new VehicleModelMakeDto
            {
                Id = v.Id,
                Name = v.Name,
                VehicleMake = v.VehicleMake,
                Year = v.Year,
                EngineCapacity = v.EngineCapacity,
                SeatingCapacity = v.SeatingCapacity,
                Fuel = v.Fuel
            })
            .ToListAsync();

            return vehicleModels;
        }

        public async Task<VehicleModelMakeDto> GetVehicleModel(int id)
        {
            var vehicleModel = await _context.VehicleModels
            .Include(v => v.VehicleMake)
            .Select (v => new VehicleModelMakeDto
            {
                Id = v.Id,
                Name = v.Name,
                VehicleMake = v.VehicleMake,
                Year = v.Year,
                EngineCapacity = v.EngineCapacity,
                SeatingCapacity = v.SeatingCapacity,
                Fuel = v.Fuel
            })
            .FirstOrDefaultAsync(v => v.Id==id);

           return vehicleModel;
        }

        public async Task<VehicleModel> CreateVehicleModel(CreateVehicleModelDto createVehicleModelDto)
        {
            var vehicleModel = new VehicleModel
            {
                Name = createVehicleModelDto.Name,
                VehicleMakeId = createVehicleModelDto.VehicleMakeId,
                Year = createVehicleModelDto.Year,
                EngineCapacity = createVehicleModelDto.EngineCapacity,
                SeatingCapacity = createVehicleModelDto.SeatingCapacity,
                Fuel = createVehicleModelDto.Fuel
            };
            _context.VehicleModels.Add(vehicleModel);
            await _context.SaveChangesAsync();
            return vehicleModel;
        }

        public async Task<VehicleModel> UpdateVehicleModel(int id, CreateVehicleModelDto vehicleModel)
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
