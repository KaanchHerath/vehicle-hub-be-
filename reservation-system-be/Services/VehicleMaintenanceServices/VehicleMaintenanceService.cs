using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.VehicleMaintenanceServices
{
    public class VehicleMaintenanceService : IVehicleMaintenanceService
    {
        private readonly DataContext _context;
        private readonly IVehicleService _vehicleService;

        public VehicleMaintenanceService(DataContext context, IVehicleService vehicleService)
        {
            _context = context;
            _vehicleService = vehicleService;
        }

        public async Task<IEnumerable<VehicleMaintenanceDto>> GetAllVehicleMaintenances()
        {
            var vehicleMaintenances = await _context.VehicleMaintenances
            .Include(v => v.Vehicle)
            .Select(v => new VehicleMaintenanceDto
            {
                Id = v.Id,
                Date = v.Date,
                Description = v.Description,
                Type = v.Type,
                CurrentMileage = v.CurrentMileage,
                VehicleId = v.VehicleId,
                RegistrationNumber = v.Vehicle.RegistrationNumber
            })
            .OrderByDescending(v => v.Id)
            .ToListAsync();

            return vehicleMaintenances;

            /*if (vehicleMaintenances == null || !vehicleMaintenances.Any())
            {
                throw new DataNotFoundException("No vehicle maintenances found");
            }

            var vehicleMaintenanceDtos = new List<VehicleMaintenanceDto>();

            foreach (var vehicleMaintenance in vehicleMaintenances)
            {
                var vehicle = await _vehicleService.GetVehicle(vehicleMaintenance.VehicleId);

                var vehicleMaintenanceDto = new VehicleMaintenanceDto
                {
                    Id = vehicleMaintenance.Id,
                    Date = vehicleMaintenance.Date,
                    Description = vehicleMaintenance.Description,
                    Type = vehicleMaintenance.Type,
                    CurrentMileage = vehicleMaintenance.CurrentMileage,
                    Vehicle = vehicle
                };

                vehicleMaintenanceDtos.Add(vehicleMaintenanceDto);
            }
            vehicleMaintenanceDtos = vehicleMaintenanceDtos.OrderByDescending(v => v.Id).ToList();
            return vehicleMaintenanceDtos;*/
        }

        public async Task<VehicleMaintenanceDto> GetVehicleMaintenance(int id)
        {
            var vehicleMaintenance = await _context.VehicleMaintenances
                .Include(v => v.Vehicle)
                .Select(v => new VehicleMaintenanceDto
                {
                    Id = v.Id,
                    Date = v.Date,
                    Description = v.Description,
                    Type = v.Type,
                    CurrentMileage = v.CurrentMileage,
                    VehicleId = v.VehicleId,
                    RegistrationNumber = v.Vehicle.RegistrationNumber
                })
                .FirstOrDefaultAsync(v => v.Id == id);

                return vehicleMaintenance;

            /*if (vehicleMaintenance == null)
            {
                throw new DataNotFoundException("Vehicle maintenance not found");
            }
            var vehicleMaintenanceDto = new VehicleMaintenanceDto
            {
                Id = vehicleMaintenance.Id,
                Date = vehicleMaintenance.Date,
                Description = vehicleMaintenance.Description,
                Type = vehicleMaintenance.Type,
                CurrentMileage = vehicleMaintenance.CurrentMileage,
                Vehicle = await _vehicleService.GetVehicle(vehicleMaintenance.VehicleId)
            };
            return vehicleMaintenanceDto;*/
        }

        public async Task<VehicleMaintenance> CreateVehicleMaintenance(CreateVehicleMaintenanceDto CreatevehicleMaintenanceDto)
        {
            var vehicleMaintenance = new VehicleMaintenance
            {
                Date = CreatevehicleMaintenanceDto.Date,
                Description = CreatevehicleMaintenanceDto.Description,
                Type = CreatevehicleMaintenanceDto.Type,
                CurrentMileage = CreatevehicleMaintenanceDto.CurrentMileage,
                VehicleId = CreatevehicleMaintenanceDto.VehicleId
            };
            _context.VehicleMaintenances.Add(vehicleMaintenance);
            await _context.SaveChangesAsync();

            var vehicle = await _context.Vehicles.FindAsync(vehicleMaintenance.VehicleId);
            if(vehicle == null)
            {
                throw new DataNotFoundException();
            }
            vehicle.Mileage = vehicleMaintenance.CurrentMileage;
            _context.Entry(vehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return vehicleMaintenance;
        }

        public async Task<VehicleMaintenance> UpdateVehicleMaintenance(int id, CreateVehicleMaintenanceDto vehicleMaintenance)
        {
            var existingVehicleMaintenance = await _context.VehicleMaintenances.FindAsync(id);
            if (existingVehicleMaintenance == null)
            {
                throw new DataNotFoundException("Vehicle maintenance not found");
            }
            existingVehicleMaintenance.Date = vehicleMaintenance.Date;
            existingVehicleMaintenance.Description = vehicleMaintenance.Description;
            existingVehicleMaintenance.Type = vehicleMaintenance.Type;
            existingVehicleMaintenance.CurrentMileage = vehicleMaintenance.CurrentMileage;
            existingVehicleMaintenance.VehicleId = vehicleMaintenance.VehicleId;
            _context.Entry(existingVehicleMaintenance).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicleMaintenance;
        }

        public async Task DeleteVehicleMaintenance(int id)
        {
            var vehicleMaintenance = await _context.VehicleMaintenances.FindAsync(id);
            if (vehicleMaintenance == null)
            {
                throw new DataNotFoundException("Vehicle maintenance not found");
            }
            _context.VehicleMaintenances.Remove(vehicleMaintenance);
            await _context.SaveChangesAsync();
        }
    }
}
