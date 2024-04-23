using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleMaintenanceServices
{
    public class VehicleMaintenanceService : IVehicleMaintenanceService
    {
        private readonly DataContext _context;

        public VehicleMaintenanceService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleMaintenance>> GetAllVehicleMaintenances()
        {
            return await _context.VehicleMaintenances.ToListAsync();
        }

        public async Task<VehicleMaintenance> GetVehicleMaintenance(int id)
        {
            var vehicleMaintenance = await _context.VehicleMaintenances.FindAsync(id);
            if (vehicleMaintenance == null)
            {
                throw new DataNotFoundException("Vehicle maintenance not found");
            }
            return vehicleMaintenance;
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
