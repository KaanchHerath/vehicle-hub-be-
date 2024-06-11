using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.DTOs;
using reservation_system_be.Services.EmployeeServices;
using reservation_system_be.Services.VehicleModelServices;
using reservation_system_be.Services.VehicleTypeServices;
using reservation_system_be.Services.FileServices;
using Microsoft.AspNetCore.Mvc;

namespace reservation_system_be.Services.VehicleServices
{
    public class VehicleService : IVehicleService
    {
        private readonly DataContext _context;
        private readonly IVehicleTypeService _vehicleTypeService;
        private readonly IVehicleModelService _vehicleModelService;
        private readonly IEmployeeService _employeeService;
        private const string AzureContainerName = "thumbnails";
        private const string AzureContainerName2 = "front";
        private const string AzureContainerName3 = "rear";
        private const string AzureContainerName4 = "dashboard";
        private const string AzureContainerName5 = "interior";
        private readonly IFileService _fileServices;

        public VehicleService(DataContext context, IVehicleTypeService vehicleTypeService, IVehicleModelService vehicleModelService, IEmployeeService employeeService, IFileService fileService)
        {
            _context = context;
            _vehicleTypeService = vehicleTypeService;
            _vehicleModelService = vehicleModelService;
            _employeeService = employeeService;
            _fileServices = fileService;
        }
        public async Task<IEnumerable<VehicleDto>> GetAllVehicles()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .Include(v => v.Employee)
                .ToListAsync();

            if (vehicles == null || !vehicles.Any())
            {
                throw new DataNotFoundException("No vehicles found");
            }

            var vehicleDtos = new List<VehicleDto>();
            foreach (var vehicle in vehicles)
            {
                var vehicleType = await _vehicleTypeService.GetSingleVehicleType(vehicle.VehicleTypeId);
                var vehicleModel = await _vehicleModelService.GetVehicleModel(vehicle.VehicleModelId);
                var employee = await _employeeService.GetEmployee(vehicle.EmployeeId);

                var vehicleDto = new VehicleDto
                {
                    Id = vehicle.Id,
                    RegistrationNumber = vehicle.RegistrationNumber,
                    ChassisNo = vehicle.ChassisNo,
                    Colour = vehicle.Colour,
                    Mileage = vehicle.Mileage,
                    CostPerDay = vehicle.CostPerDay,
                    Transmission = vehicle.Transmission,
                    CostPerExtraKM = vehicle.CostPerExtraKM,
                    Thumbnail = vehicle.Thumbnail,
                    FrontImg = vehicle.FrontImg,
                    RearImg = vehicle.RearImg,
                    DashboardImg = vehicle.DashboardImg,
                    InteriorImg = vehicle.InteriorImg,
                    Status = vehicle.Status,
                    VehicleType = vehicleType,
                    VehicleModel = vehicleModel,
                    Employee = employee
                };
                vehicleDtos.Add(vehicleDto);
            }
            return vehicleDtos;
        }

        public async Task<VehicleDto> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .Include(v => v.Employee)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }
            var vehicleDto = new VehicleDto
            {
                Id = vehicle.Id,
                RegistrationNumber = vehicle.RegistrationNumber,
                ChassisNo = vehicle.ChassisNo,
                Colour = vehicle.Colour,
                Mileage = vehicle.Mileage,
                CostPerDay = vehicle.CostPerDay,
                Transmission = vehicle.Transmission,
                CostPerExtraKM = vehicle.CostPerExtraKM,
                Thumbnail = vehicle.Thumbnail,
                FrontImg = vehicle.FrontImg,
                RearImg = vehicle.RearImg,
                DashboardImg = vehicle.DashboardImg,
                InteriorImg = vehicle.InteriorImg,
                Status = vehicle.Status,
                VehicleType = await _vehicleTypeService.GetSingleVehicleType(vehicle.VehicleTypeId),
                VehicleModel = await _vehicleModelService.GetVehicleModel(vehicle.VehicleModelId),
                Employee = await _employeeService.GetEmployee(vehicle.EmployeeId)
            };
            return vehicleDto;
        }
        public async Task<Vehicle> CreateVehicle([FromForm] CreateVehicleDto createVehicleDto,IFormFile formFile,IFormFile front,IFormFile rear,IFormFile dashboard, IFormFile interior)
        {
            if (formFile == null || formFile.Length == 0)
            {
                throw new DataNotFoundException("Thumbnail is required");
            }
            if (front == null || front.Length == 0)
            {
                throw new DataNotFoundException("Front image is required");
            }
            if (rear == null || rear.Length == 0)
            {
                throw new DataNotFoundException("Rear image is required");
            }
            if (dashboard == null || dashboard.Length == 0)
            {
                throw new DataNotFoundException("Dashboard image is required");
            }
            if (interior == null || interior.Length == 0)
            {
                throw new DataNotFoundException("Interior image is required");
            }

            var fileUrl = await _fileServices.Upload(formFile, AzureContainerName);
            var frontUrl = await _fileServices.Upload(front, AzureContainerName2);
            var rearUrl = await _fileServices.Upload(rear, AzureContainerName3);
            var dashboardUrl = await _fileServices.Upload(dashboard, AzureContainerName4);
            var interiorUrl = await _fileServices.Upload(interior, AzureContainerName5);

            var vehicle = new Vehicle
            {
                RegistrationNumber = createVehicleDto.RegistrationNumber,
                ChassisNo = createVehicleDto.ChassisNo,
                Colour = createVehicleDto.Colour,
                Mileage = createVehicleDto.Mileage,
                CostPerDay = createVehicleDto.CostPerDay,
                Transmission = createVehicleDto.Transmission,
                CostPerExtraKM = createVehicleDto.CostPerExtraKM,
                Thumbnail = fileUrl,
                FrontImg = frontUrl,
                RearImg = rearUrl,
                DashboardImg = dashboardUrl,
                InteriorImg = interiorUrl,
                Status = createVehicleDto.Status,
                VehicleTypeId = createVehicleDto.VehicleTypeId,
                VehicleModelId = createVehicleDto.VehicleModelId,
                EmployeeId = createVehicleDto.EmployeeId
            };
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicle(int id, [FromForm]CreateVehicleDto createVehicleDto,IFormFile formFile, IFormFile front, IFormFile rear, IFormFile dashboard, IFormFile interior)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }

            if (formFile != null && formFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingVehicle.Thumbnail))
                {
                    await _fileServices.Delete(existingVehicle.Thumbnail, AzureContainerName);
                }
                var fileUrl = await _fileServices.Upload(formFile, AzureContainerName);
                existingVehicle.Thumbnail = fileUrl;
            }
            if(front != null && front.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingVehicle.FrontImg))
                {
                    await _fileServices.Delete(existingVehicle.FrontImg, AzureContainerName2);
                }
                var frontUrl = await _fileServices.Upload(front, AzureContainerName2);
                existingVehicle.FrontImg = frontUrl;
            }
            if (rear != null && rear.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingVehicle.RearImg))
                {
                    await _fileServices.Delete(existingVehicle.RearImg, AzureContainerName3);
                }
                var rearUrl = await _fileServices.Upload(rear, AzureContainerName3);
                existingVehicle.RearImg = rearUrl;
            }
            if (dashboard != null && dashboard.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingVehicle.DashboardImg))
                {
                    await _fileServices.Delete(existingVehicle.DashboardImg, AzureContainerName4);
                }
                var dashboardUrl = await _fileServices.Upload(dashboard, AzureContainerName4);
                existingVehicle.DashboardImg = dashboardUrl;
            }
            if (interior != null && interior.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingVehicle.InteriorImg))
                {
                    await _fileServices.Delete(existingVehicle.InteriorImg, AzureContainerName5);
                }
                var interiorUrl = await _fileServices.Upload(interior, AzureContainerName5);
                existingVehicle.InteriorImg = interiorUrl;
            }

            existingVehicle.RegistrationNumber = createVehicleDto.RegistrationNumber;
            existingVehicle.ChassisNo = createVehicleDto.ChassisNo;
            existingVehicle.Colour = createVehicleDto.Colour;
            existingVehicle.Mileage = createVehicleDto.Mileage;
            existingVehicle.CostPerDay = createVehicleDto.CostPerDay;
            existingVehicle.Transmission = createVehicleDto.Transmission;
            existingVehicle.CostPerExtraKM = createVehicleDto.CostPerExtraKM;
            existingVehicle.Status = createVehicleDto.Status;
            existingVehicle.VehicleTypeId = createVehicleDto.VehicleTypeId;
            existingVehicle.VehicleModelId = createVehicleDto.VehicleModelId;
            existingVehicle.EmployeeId = createVehicleDto.EmployeeId;
            _context.Entry(existingVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return existingVehicle;
        }
        public async Task DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }

            if (!string.IsNullOrEmpty(vehicle.Thumbnail))
            {
                await _fileServices.Delete(vehicle.Thumbnail, AzureContainerName);
            }
            if (!string.IsNullOrEmpty(vehicle.FrontImg))
            {
                await _fileServices.Delete(vehicle.FrontImg, AzureContainerName2);
            }
            if (!string.IsNullOrEmpty(vehicle.RearImg))
            {
                await _fileServices.Delete(vehicle.RearImg, AzureContainerName3);
            }
            if (!string.IsNullOrEmpty(vehicle.DashboardImg))
            {
                await _fileServices.Delete(vehicle.DashboardImg, AzureContainerName4);
            }
            if (!string.IsNullOrEmpty(vehicle.InteriorImg))
            {
                await _fileServices.Delete(vehicle.InteriorImg, AzureContainerName5);
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task<List<VehicleResponse>> SearchVehicle(string search)
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake) // Ensure VehicleMake is included
                .Where(v =>
                    v.VehicleModel.Name.Contains(search) ||
                    v.VehicleType.Name.Contains(search)
                )
                .Select(v => new VehicleResponse
                {
                    Vehicle = v,
                    vehicleMake = v.VehicleModel.VehicleMake,
                    vehicleModel = v.VehicleModel
                })
                .ToListAsync();

            return vehicles;
        }


        public async Task<List<VehicleResponse>> GetAllVehiclesDetails()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake)
                .Select(v => new VehicleResponse
                {
                    Vehicle = v,
                    vehicleMake = v.VehicleModel.VehicleMake,
                    vehicleModel = v.VehicleModel
                })
                .ToListAsync();

            return vehicles;
        }


        public async Task<List<VehicleResponse>> FilterVehicles(int? vehicleTypeId, int? vehicleMakeId, int? seatingCapacity, float? depositAmount)
        {
            var query = _context.Vehicles
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake)
                .Include(v => v.VehicleType)
                .AsQueryable();

            if (vehicleTypeId.HasValue)
            {
                query = query.Where(v => v.VehicleTypeId == vehicleTypeId.Value);
            }

            if (vehicleMakeId.HasValue)
            {
                var joinedQuery = query.Join(
                    _context.VehicleModels,
                    vehicle => vehicle.VehicleModelId,
                    vehicleModel => vehicleModel.Id,
                    (vehicle, vehicleModel) => new { vehicle, vehicleModel }
                );

                if (vehicleMakeId.HasValue)
                {
                    joinedQuery = joinedQuery.Where(vm => vm.vehicleModel.VehicleMakeId == vehicleMakeId.Value);
                }

                if (!(seatingCapacity == 0))
                {
                    joinedQuery = joinedQuery.Where(vm => vm.vehicleModel.SeatingCapacity < seatingCapacity);
                }

                query = joinedQuery.Select(vm => vm.vehicle);
            }

            if (depositAmount.HasValue)
            {
                query = query.Where(v => v.VehicleType.DepositAmount < depositAmount.Value);
            }

            var result = await query
                .Select(v => new VehicleResponse
                {
                    Vehicle = v,
                    vehicleMake = v.VehicleModel.VehicleMake,
                    vehicleModel = v.VehicleModel
                })
                .ToListAsync();

            return result;
        }


    }

    public class VehicleResponse
    {
        public Vehicle Vehicle { get; set; }

        public VehicleMake vehicleMake { get; set; }

        public VehicleModel vehicleModel { get; set; }
    }

}
