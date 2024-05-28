using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.CustomerReservationService;

namespace reservation_system_be.Services.VehicleLogServices
{
    public class VehicleLogService : IVehicleLogService
    {
        private readonly DataContext _context;
        private readonly ICustomerReservationService _customerReservationService;

        public VehicleLogService(DataContext context, ICustomerReservationService customerReservationService)
        {
            _context = context;
            _customerReservationService = customerReservationService;
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

        public async Task<VehicleLog> UpdateVehicleLog(int id, VehicleLogDto vehicleLog)
        {
            var existingVehicleLog = await _context.VehicleLogs.FindAsync(id);
            if (existingVehicleLog == null)
            {
                throw new DataNotFoundException("Vehicle log not found");
            }
            var customerReservation = await _customerReservationService.GetCustomerReservation(vehicleLog.CustomerReservationId);

            var KM = vehicleLog.EndMileage - customerReservation.Vehicle.Mileage;
            var ExtraKM = KM - (customerReservation.Reservation.NoOfDays * 100);

            existingVehicleLog.EndMileage = vehicleLog.EndMileage;
            existingVehicleLog.Penalty = vehicleLog.Penalty;
            existingVehicleLog.Description = vehicleLog.Description;
            existingVehicleLog.ExtraKM = KM < 0 ? 0 : ExtraKM;
            existingVehicleLog.CustomerReservationId = vehicleLog.CustomerReservationId;
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
