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

        public async Task<IEnumerable<VehicleModelDto>> GetAllVehicleModels()
        {
            var vehicleModels = await _context.VehicleModels
            .Include(v => v.VehicleMake)
            .ToListAsync();

            if (vehicleModels == null || !vehicleModels.Any())
            {
                throw new DataNotFoundException("No vehicle models found");
            }

            var  vehicleModelDtos = new List<VehicleModelDto>();

            foreach (var vehicleModel in vehicleModels)
            {
                var vehicleMake = await _vehicleMakeService.GetVehicleMake(vehicleModel.VehicleMakeId);

                var vehicleModelDto = new VehicleModelDto
                {
                    Id = vehicleModel.Id,
                    Name = vehicleModel.Name,
                    VehicleMake = vehicleMake,
                    Year = vehicleModel.Year,
                    EngineCapacity = vehicleModel.EngineCapacity,
                    SeatingCapacity = vehicleModel.SeatingCapacity,
                    Fuel = vehicleModel.Fuel
                };

                vehicleModelDtos.Add(vehicleModelDto);
            }

            return vehicleModelDtos;
        }

        public async Task<VehicleModelDto> GetVehicleModel(int id)
        {
            var vehicleModel = await _context.VehicleModels
            .Include(v => v.VehicleMake)
            .FirstOrDefaultAsync(v => v.Id==id);

            if (vehicleModel == null)
            {
                throw new DataNotFoundException("Vehicle model not found");
            }

            var vehicleModelDto = new VehicleModelDto
            {
                Id = vehicleModel.Id,
                Name = vehicleModel.Name,
                VehicleMake = await _vehicleMakeService.GetVehicleMake(vehicleModel.VehicleMakeId),
                Year = vehicleModel.Year,
                EngineCapacity = vehicleModel.EngineCapacity,
                SeatingCapacity = vehicleModel.SeatingCapacity,
                Fuel = vehicleModel.Fuel

            };

            return vehicleModelDto;
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
