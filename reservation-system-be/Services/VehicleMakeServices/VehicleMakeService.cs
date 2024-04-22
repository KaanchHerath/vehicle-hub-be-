using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleMakeServices
{
    public class VehicleMakeService : IVehicleMakeService
    {
        private readonly DataContext _context;

        public VehicleMakeService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleMake>> GetAllVehicleMakes()
        {
            return await _context.VehicleMake.ToListAsync();
        }

        public async Task<VehicleMake> GetVehicleMake(int id)
        {
            var vehicleMake = await _context.VehicleMake.FindAsync(id);
            if (vehicleMake == null)
            {
                throw new DataNotFoundException("Vehicle make not found");
            }
            return vehicleMake;
        }

        public async Task<VehicleMake> CreateVehicleMake(VehicleMake vehicleMake)
        {
            _context.VehicleMake.Add(vehicleMake);
            await _context.SaveChangesAsync();
            return vehicleMake;
        }

        public async Task<VehicleMake> UpdateVehicleMake(int id, VehicleMake vehicleMake)
        {
            var existingVehicleMake = await _context.VehicleMake.FindAsync(id);
            if (existingVehicleMake == null)
            {
                throw new DataNotFoundException("Vehicle make not found");
            }
            existingVehicleMake.Name = vehicleMake.Name;
            existingVehicleMake.Logo = vehicleMake.Logo;
            _context.Entry(existingVehicleMake).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleMake;
        }

        public async Task DeleteVehicleMake(int id)
        {
            var vehicleMake = await _context.VehicleMake.FindAsync(id);
            if (vehicleMake == null)
            {
                throw new DataNotFoundException("Vehicle make not found");
            }
            _context.VehicleMake.Remove(vehicleMake);
            await _context.SaveChangesAsync();
        }
    }
}