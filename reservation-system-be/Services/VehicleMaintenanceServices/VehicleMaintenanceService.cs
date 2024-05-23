using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.VehicleMaintenanceServices
{
    public class VehicleMaintenanceService : IVehicleMaintenanceService
    {
        private readonly DataContext _context;
        private readonly IVehicleService _vehicleService;

        public VehicleMaintenanceService(DataContext context, IVehicleService vehicleService)
        {
            _context = context;
            _vehicleService = vehicleService;
        }

        public async Task<IEnumerable<VehicleMaintenanceDto>> GetAllVehicleMaintenances()
        {
            var vehicleMaintenances = await _context.VehicleMaintenances
            .Include(v => v.Vehicle)
            .ToListAsync();

            if (vehicleMaintenances == null || !vehicleMaintenances.Any())
            {
                throw new DataNotFoundException("No vehicle maintenances found");
            }

            var vehicleMaintenanceDtos = new List<VehicleMaintenanceDto>();

            foreach (var vehicleMaintenance in vehicleMaintenances)
            {
                var vehicle = await _vehicleService.GetVehicle(vehicleMaintenance.VehicleId);

                var vehicleMaintenanceDto = new VehicleMaintenanceDto
                {
                    Id = vehicleMaintenance.Id,
                    Date = vehicleMaintenance.Date,
                    Description = vehicleMaintenance.Description,
                    Type = vehicleMaintenance.Type,
                    Vehicle = vehicle
                };

                vehicleMaintenanceDtos.Add(vehicleMaintenanceDto);
            }
            return vehicleMaintenanceDtos;
        }

        public async Task<VehicleMaintenanceDto> GetVehicleMaintenance(int id)
        {
            var vehicleMaintenance = await _context.VehicleMaintenances
                .Include(v => v.Vehicle)
                .FirstOrDefaultAsync();

            if (vehicleMaintenance == null)
            {
                throw new DataNotFoundException("Vehicle maintenance not found");
            }
            var vehicleMaintenanceDto = new VehicleMaintenanceDto
            {
                Id = vehicleMaintenance.Id,
                Date = vehicleMaintenance.Date,
                Description = vehicleMaintenance.Description,
                Type = vehicleMaintenance.Type,
                Vehicle = await _vehicleService.GetVehicle(vehicleMaintenance.VehicleId)
            };
            return vehicleMaintenanceDto;
        }

        public async Task<VehicleMaintenance> CreateVehicleMaintenance(VehicleMaintenance vehicleMaintenance)
        {
            _context.VehicleMaintenances.Add(vehicleMaintenance);
            await _context.SaveChangesAsync();
            return vehicleMaintenance;
        }

        public async Task<VehicleMaintenance> UpdateVehicleMaintenance(int id, VehicleMaintenance vehicleMaintenance)
        {
            var existingVehicleMaintenance = await _context.VehicleMaintenances.FindAsync(id);
            if (existingVehicleMaintenance == null)
            {
                throw new DataNotFoundException("Vehicle maintenance not found");
            }
            existingVehicleMaintenance.Date = vehicleMaintenance.Date;
            existingVehicleMaintenance.Description = vehicleMaintenance.Description;
            existingVehicleMaintenance.Type = vehicleMaintenance.Type;
            existingVehicleMaintenance.VehicleId = vehicleMaintenance.VehicleId;
            _context.Entry(existingVehicleMaintenance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleMaintenance;
        }

        public async Task DeleteVehicleMaintenance(int id)
        {
            var vehicleMaintenance = await _context.VehicleMaintenances.FindAsync(id);
            if (vehicleMaintenance == null)
            {
                throw new DataNotFoundException("Vehicle maintenance not found");
            }
            _context.VehicleMaintenances.Remove(vehicleMaintenance);
            await _context.SaveChangesAsync();
        }
    }
}
