using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleLogServices
{
    public class VehicleLogService : IVehicleLogService
    {
        private readonly DataContext _context;

        public VehicleLogService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleLog>> GetAllVehicleLogs()
        {
            return await _context.VehicleLogs.ToListAsync();
        }

        public async Task<VehicleLog> GetVehicleLog(int id)
        {
            var vehicleLog = await _context.VehicleLogs.FindAsync(id);
            if (vehicleLog == null)
            {
                throw new DataNotFoundException("Vehicle log not found");
            }
            return vehicleLog;
        }

        public async Task<VehicleLog> CreateVehicleLog(VehicleLog vehicleLog)
        {
            _context.VehicleLogs.Add(vehicleLog);
            await _context.SaveChangesAsync();
            return vehicleLog;
        }

        public async Task<VehicleLog> UpdateVehicleLog(int id, VehicleLog vehicleLog)
        {
            var existingVehicleLog = await _context.VehicleLogs.FindAsync(id);
            if (existingVehicleLog == null)
            {
                throw new DataNotFoundException("Vehicle log not found");
            }
            existingVehicleLog.EndMileage = vehicleLog.EndMileage;
            existingVehicleLog.Penalty = vehicleLog.Penalty;
            existingVehicleLog.Description = vehicleLog.Description;
            existingVehicleLog.ExtraDays = vehicleLog.ExtraDays;
            existingVehicleLog.ReservationId = vehicleLog.ReservationId;
            _context.Entry(existingVehicleLog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleLog;
        }


        public async Task DeleteVehicleLog(int id)
        {
            var vehicleLog = await _context.VehicleLogs.FindAsync(id);
            if (vehicleLog == null)
            {
                throw new DataNotFoundException("Vehicle log not found");
            }
            _context.VehicleLogs.Remove(vehicleLog);
            await _context.SaveChangesAsync();
        }
    }
}
