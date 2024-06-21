using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.VehicleInsuranceServices
{
    public class VehicleInsuranceService : IVehicleInsuranceService
    {
        private readonly DataContext _context;
        private readonly IVehicleService _vehicleService;

        public VehicleInsuranceService(DataContext context, IVehicleService vehicleService)
        {
            _context = context;
            _vehicleService = vehicleService;
        }
        public async Task<IEnumerable<VehicleInsuranceDto>> GetAllVehicleInsurances()
        {
            var vehicleInsurances = await _context.VehicleInsurances
            .Include(vi => vi.Vehicle)
            .Select(vi => new VehicleInsuranceDto
            {
                Id = vi.Id,
                InsuranceNo = vi.InsuranceNo,
                ExpiryDate = vi.ExpiryDate,
                VehicleId = vi.VehicleId,
                RegistrationNo = vi.Vehicle.RegistrationNumber,
                Status = vi.Status
            })
            .ToListAsync();

            return vehicleInsurances;
        }
        public async Task<VehicleInsuranceDto> GetSingleVehicleInsurance(int id)
        {
            var vehicleInsurance = await _context.VehicleInsurances
            .Include(vi => vi.Vehicle)
            .Select(vi => new VehicleInsuranceDto
             {
                 Id = vi.Id,
                 InsuranceNo = vi.InsuranceNo,
                 ExpiryDate = vi.ExpiryDate,
                 VehicleId = vi.VehicleId,
                 RegistrationNo = vi.Vehicle.RegistrationNumber,
                 Status = vi.Status
             })
            .FirstAsync(vi => vi.Id == id);

            return vehicleInsurance;
        }
        public async Task<CreateVehicleInsuranceDto> CreateVehicleInsurance(CreateVehicleInsuranceDto vehicleInsuranceDto)
        {
            var vehicleInsurance = new VehicleInsurance
            {
                InsuranceNo = vehicleInsuranceDto.InsuranceNo,
                ExpiryDate = vehicleInsuranceDto.ExpiryDate,
                VehicleId = vehicleInsuranceDto.VehicleId
            };

            _context.VehicleInsurances.Add(vehicleInsurance);
            await _context.SaveChangesAsync();

            return vehicleInsuranceDto;
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
