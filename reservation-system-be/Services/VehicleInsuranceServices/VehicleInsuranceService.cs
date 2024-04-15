using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;

namespace reservation_system_be.Services.VehicleInsuranceServices
{
    public class VehicleInsuranceService : IVehicleInsuranceService
    {
        private readonly DataContext _context;

        public VehicleInsuranceService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleInsurance>> GetAllVehicleInsurance()
        {
            return await _context.VehicleInsurances.ToListAsync();
        }

        public async Task<VehicleInsurance?> GetSingleVehicleInsurance(int id)
        {
            return await _context.VehicleInsurances.FindAsync(id);
        }
        public async Task<List<VehicleInsurance>> AddVehicleInsurance(VehicleInsurance vehicleInsurance)
        {
            _context.VehicleInsurances.Add(vehicleInsurance);
            await _context.SaveChangesAsync();
            return await _context.VehicleInsurances.ToListAsync();
        }

        public async Task<List<VehicleInsurance>?> UpdateVehicleInsurance(int id, VehicleInsurance request)
        {
            var vehicleInsurance = await _context.VehicleInsurances.FindAsync(id);
            if (vehicleInsurance == null)
            {
                return null;
            }

            vehicleInsurance.InsuranceNo = request.InsuranceNo;
            vehicleInsurance.ExpiryDate = request.ExpiryDate;
            vehicleInsurance.VehicleId = request.VehicleId;
            await _context.SaveChangesAsync();
            return await _context.VehicleInsurances.ToListAsync();
                      }

        public async Task<List<VehicleInsurance>?> DeleteVehicleInsurance(int id)
        {
            var vehicleInsurance = await _context.VehicleInsurances.FindAsync(id);
            if (vehicleInsurance == null)
            {
                return null;
            }

            _context.VehicleInsurances.Remove(vehicleInsurance);
            await _context.SaveChangesAsync();
            return await _context.VehicleInsurances.ToListAsync();
        }
    }
}
