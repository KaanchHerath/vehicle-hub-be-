using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.MaintenanceTypeServices
{
    public class MaintenanceTypeService : IMaintenanceTypeService
    {
        private readonly DataContext _context;

        public MaintenanceTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<MaintenanceType>> GetAllMaintenanceType()
        {
            return await _context.MaintenanceTypes.ToListAsync();
        }

        public async Task<MaintenanceType?> GetSingleMaintenanceType(int id)
        {
            return await _context.MaintenanceTypes.FindAsync(id);
        }

        public async Task<List<MaintenanceType>> AddMaintenanceType(MaintenanceType maintenanceType)
        {
            _context.MaintenanceTypes.Add(maintenanceType);
            await _context.SaveChangesAsync();
            return await _context.MaintenanceTypes.ToListAsync();
        }

        public async Task<List<MaintenanceType>?> UpdateMaintenanceType(int id, MaintenanceType request)
        {
            var maintenanceType = await _context.MaintenanceTypes.FindAsync(id);
            if (maintenanceType == null)
            {
                return null;
            }

            maintenanceType.Name = request.Name;
            maintenanceType.MaintenanceId = request.MaintenanceId;
            await _context.SaveChangesAsync();
            return await _context.MaintenanceTypes.ToListAsync();
        }

        public async Task<List<MaintenanceType>?> DeleteMaintenanceType(int id)
        {
            var maintenanceType = await _context.MaintenanceTypes.FindAsync(id);
            if (maintenanceType == null)
            {
                return null;
            }

            _context.MaintenanceTypes.Remove(maintenanceType);
            await _context.SaveChangesAsync();
            return await _context.MaintenanceTypes.ToListAsync();
        }
    }
}
