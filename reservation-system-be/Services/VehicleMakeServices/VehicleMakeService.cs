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

        public async Task<List<VehicleMake>> GetAllVehicleMake()
        {
            return await _context.VehicleMakes.ToListAsync();
        }

        public async Task<VehicleMake?> GetSingleVehicleMake(int id)
        {
            return await _context.VehicleMakes.FindAsync(id);
        }

        public async Task<List<VehicleMake>> AddVehicleMake(VehicleMake vehicleMake)
        {
            _context.VehicleMakes.Add(vehicleMake);
            await _context.SaveChangesAsync();
            return await _context.VehicleMakes.ToListAsync();
        }

        public async Task<List<VehicleMake>?> UpdateVehicleMake(int id, VehicleMake request)
        {
            var vehicleMake = await _context.VehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return null;
            }

            vehicleMake.Name = request.Name;
            vehicleMake.Logo = request.Logo;
            await _context.SaveChangesAsync();
            return await _context.VehicleMakes.ToListAsync();
        }

        public async Task<List<VehicleMake>?> DeleteVehicleMake(int id)
        {
            var vehicleMake = await _context.VehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return null;
            }

            _context.VehicleMakes.Remove(vehicleMake);
            await _context.SaveChangesAsync();
            return await _context.VehicleMakes.ToListAsync();
        }
    }
}
