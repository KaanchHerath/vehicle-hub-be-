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
        public async Task<List<VehicleInsurance>> GetAllVehicleInsurances()
        {
            return await _context.VehicleInsurances.ToListAsync();
        }
        public async Task<VehicleInsurance> GetSingleVehicleInsurance(int id)
        {
            var vehicleInsurance = await _context.VehicleInsurances.FindAsync(id);
            if (vehicleInsurance == null)
            {
                throw new DataNotFoundException("Vehicle Insurance not found");
            }
            return vehicleInsurance;
        }
        public async Task<VehicleInsurance> CreateVehicleInsurance(VehicleInsurance vehicleInsurance)
        {
            _context.VehicleInsurances.Add(vehicleInsurance);
            await _context.SaveChangesAsync();
            return vehicleInsurance;
        }

        public async Task<VehicleInsurance> UpdateVehicleInsurance(int id, VehicleInsurance vehicleInsurance)
        {
            var existingVehicleInsurance = await _context.VehicleInsurances.FindAsync(id);
            if (existingVehicleInsurance == null)
            {
                throw new DataNotFoundException("Vehicle Insurance not found");
            }
            existingVehicleInsurance.InsuranceNo = vehicleInsurance.InsuranceNo;
            existingVehicleInsurance.ExpiryDate = vehicleInsurance.ExpiryDate;
            existingVehicleInsurance.VehicleId = vehicleInsurance.VehicleId;
            _context.Entry(existingVehicleInsurance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleInsurance;
        }
        public async Task DeleteVehicleInsurance(int id)
        {
            var vehicleInsurance = await _context.VehicleInsurances.FindAsync(id);
            if (vehicleInsurance == null)
            {
                throw new DataNotFoundException("Vehicle Insurance not found");
            }
            _context.VehicleInsurances.Remove(vehicleInsurance);
            await _context.SaveChangesAsync();
        }
    }
}
