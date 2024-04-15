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

        public async Task<List<VehicleType>> GetAllVehicleType()
        {
            return await _context.VehicleTypes.ToListAsync();
        }

        public async Task<VehicleType?> GetSingleVehicleType(int id)
        {
            return await _context.VehicleTypes.FindAsync(id);
        }
        public async Task<List<VehicleType>> AddVehicleType(VehicleType vehicleType)
        {
            _context.VehicleTypes.Add(vehicleType);
            await _context.SaveChangesAsync();
            return await _context.VehicleTypes.ToListAsync();
        }
        public async Task<List<VehicleType>?> UpdateVehicleType(int id, VehicleType request)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id); 
            if (vehicleType == null)
            {
                return null;
            }

            vehicleType.Name = request.Name;
            vehicleType.DepositAmount = request.DepositAmount;
            await _context.SaveChangesAsync();
            return await _context.VehicleTypes.ToListAsync();

        }
        public async Task<List<VehicleType>?> DeleteVehicleType(int id)
        {
           var vehicleType = await _context.VehicleTypes.FindAsync(id)
           if(vehicleType == null)
           {
                return null;
           }
           _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return await _context.VehicleTypes.ToListAsync();

        }
    }
}
