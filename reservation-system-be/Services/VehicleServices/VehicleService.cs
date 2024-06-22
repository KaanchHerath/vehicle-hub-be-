using Microsoft.EntityFrameworkCore;
using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.DTOs;
using reservation_system_be.Services.EmployeeServices;
using reservation_system_be.Services.VehicleModelServices;
using reservation_system_be.Services.VehicleTypeServices;
using reservation_system_be.Services.FileServices;
using Microsoft.AspNetCore.Mvc;
using reservation_system_be.Services.NotificationServices;
using System;
using Microsoft.AspNetCore.Http.HttpResults;

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
                .Include(v => v.Employee)
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake)
                .Select (v => new VehicleDto
                {
                    Id = v.Id,
                    RegistrationNumber = v.RegistrationNumber,
                    ChassisNo = v.ChassisNo,
                    Colour = v.Colour,
                    Mileage = v.Mileage,
                    CostPerDay = v.CostPerDay,
                    Transmission = v.Transmission,
                    CostPerExtraKM = v.CostPerExtraKM,
                    Thumbnail = v.Thumbnail,
                    FrontImg = v.FrontImg,
                    RearImg = v.RearImg,
                    DashboardImg = v.DashboardImg,
                    InteriorImg = v.InteriorImg,
                    Status = v.Status,
                    Employee = v.Employee,
                    VehicleType = v.VehicleType,
                    VehicleModel = new VehicleModelMakeDto
                    {
                        Id = v.VehicleModel.Id,
                        Name = v.VehicleModel.Name,
                        EngineCapacity = v.VehicleModel.EngineCapacity,
                        SeatingCapacity = v.VehicleModel.SeatingCapacity,
                        Year = v.VehicleModel.Year,
                        Fuel = v.VehicleModel.Fuel,
                        VehicleMake = v.VehicleModel.VehicleMake
                    },
                })
                .ToListAsync();

                return vehicles;
        }

        public async Task<VehicleDto> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicles
               .Include(v => v.Employee)
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .ThenInclude(vm => vm.VehicleMake)
                .Select(v => new VehicleDto
                {
                    Id = v.Id,
                    RegistrationNumber = v.RegistrationNumber,
                    ChassisNo = v.ChassisNo,
                    Colour = v.Colour,
                    Mileage = v.Mileage,
                    CostPerDay = v.CostPerDay,
                    Transmission = v.Transmission,
                    CostPerExtraKM = v.CostPerExtraKM,
                    Thumbnail = v.Thumbnail,
                    FrontImg = v.FrontImg,
                    RearImg = v.RearImg,
                    DashboardImg = v.DashboardImg,
                    InteriorImg = v.InteriorImg,
                    Status = v.Status,
                    Employee = v.Employee,
                    VehicleType = v.VehicleType,
                    VehicleModel = new VehicleModelMakeDto
                    {
                        Id = v.VehicleModel.Id,
                        Name = v.VehicleModel.Name,
                        EngineCapacity = v.VehicleModel.EngineCapacity,
                        SeatingCapacity = v.VehicleModel.SeatingCapacity,
                        Year = v.VehicleModel.Year,
                        Fuel = v.VehicleModel.Fuel,
                        VehicleMake = v.VehicleModel.VehicleMake
                    },
                })
                .FirstOrDefaultAsync(v => v.Id == id);

                 return vehicle;
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

            var existingVehicles = await _context.Vehicles.ToListAsync();
            if (existingVehicles.Any(v => v.RegistrationNumber == vehicle.RegistrationNumber))
            {
                throw new Exception("Vehicle with the same registration number already exists");
            }

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();

            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicle(int id,UpdateVehicleDetailsDto createVehicleDto)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
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

        public async Task UpdateThumbnail(int id, IFormFile formFile)
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
            _context.Entry(existingVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFrontImg(int id, IFormFile front)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
            }
            if (front != null && front.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingVehicle.FrontImg))
                {
                    await _fileServices.Delete(existingVehicle.FrontImg, AzureContainerName2);
                }
                var frontUrl = await _fileServices.Upload(front, AzureContainerName2);
                existingVehicle.FrontImg = frontUrl;
            }
            _context.Entry(existingVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRearImg(int id, IFormFile rear)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
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
            _context.Entry(existingVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDashboardImg(int id, IFormFile dashboard)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
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
            _context.Entry(existingVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateInteriorImg(int id, IFormFile interior)
        {
            var existingVehicle = await _context.Vehicles.FindAsync(id);
            if (existingVehicle == null)
            {
                throw new DataNotFoundException("Vehicle not found");
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
            _context.Entry(existingVehicle).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
                    vehicleModel = v.VehicleModel,
                    vehicleType = v.VehicleType
                })
                .ToListAsync();

            return vehicles;
        }


    }

    public class VehicleResponse
    {
        public Vehicle Vehicle { get; set; }
        public VehicleMake vehicleMake { get; set; }
        public VehicleModel vehicleModel { get; set; }
        public VehicleType vehicleType { get; set; }
    }

}
