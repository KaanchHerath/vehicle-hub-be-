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
            .ToListAsync();
            
            if (vehicleInsurances == null || !vehicleInsurances.Any())
            {
                throw new DataNotFoundException("No vehicle insurances found");
            }

            var vehicleInsuranceDtos = new List<VehicleInsuranceDto>();

            foreach (var vehicleInsurance in vehicleInsurances)
            {
                var vehicle = await _vehicleService.GetVehicle(vehicleInsurance.VehicleId);
                var vehicleInsuranceDto = new VehicleInsuranceDto
                {
                    Id = vehicleInsurance.Id,
                    InsuranceNo = vehicleInsurance.InsuranceNo,
                    ExpiryDate = vehicleInsurance.ExpiryDate,
                    Vehicle = vehicle,
                    Status = vehicleInsurance.Status
                };

                vehicleInsuranceDtos.Add(vehicleInsuranceDto);
            }
            return vehicleInsuranceDtos;
        }
        public async Task<VehicleInsuranceDto> GetSingleVehicleInsurance(int id)
        {
            var vehicleInsurance = await _context.VehicleInsurances
            .Include(vi => vi.Vehicle)
            .FirstOrDefaultAsync(vi => vi.Id == id);

            if (vehicleInsurance == null)
            {
                throw new DataNotFoundException("Vehicle Insurance not found");
            }

            var vehicleInsuranceDto = new VehicleInsuranceDto
            {
                Id = vehicleInsurance.Id,
                InsuranceNo = vehicleInsurance.InsuranceNo,
                ExpiryDate = vehicleInsurance.ExpiryDate,
                Vehicle = await _vehicleService.GetVehicle(vehicleInsurance.VehicleId),
                Status = vehicleInsurance.Status
            };
            return vehicleInsuranceDto;
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
