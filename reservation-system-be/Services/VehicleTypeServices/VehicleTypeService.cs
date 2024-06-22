using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleTypeServices
{
    public class VehicleTypeService : IVehicleTypeService
    {
        private readonly DataContext _context;

        public VehicleTypeService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<VehicleType>> GetAllVehicleTypes()
        {
            var types = await _context.VehicleTypes.ToListAsync();
            types = types.OrderByDescending(t => t.Id).ToList();
            return types;
        }

        public async Task<VehicleType?> GetSingleVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                throw new DataNotFoundException("Vehicle type not found");
            }
            return vehicleType;
        }

        public async Task<VehicleType> CreateVehicleType(VehicleType vehicleType)
        {
            _context.VehicleTypes.Add(vehicleType);
            await _context.SaveChangesAsync();
            return vehicleType;
        }

        public async Task<VehicleType> UpdateVehicleType(int id, VehicleType vehicleType)
        {
            var existingVehicleType = await _context.VehicleTypes.FindAsync(id);
            if (existingVehicleType == null)
            {
                throw new DataNotFoundException("Vehicle type not found");
            }
            existingVehicleType.Name = vehicleType.Name;
            existingVehicleType.DepositAmount = vehicleType.DepositAmount;
            _context.Entry(existingVehicleType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleType;
        }

        public async Task DeleteVehicleType(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                throw new DataNotFoundException("Vehicle type not found");
            }
            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
        }
    } 
}
