using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;
using reservation_system_be.Services.EmployeeServices;
using reservation_system_be.Services.FileServices;
using reservation_system_be.Services.VehicleModelServices;
using reservation_system_be.Services.VehicleTypeServices;

namespace reservation_system_be.Services.VehicleFilterServices
{
    public class VehicleFilterService : IVehicleFilterService
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

        public VehicleFilterService(DataContext context, IVehicleTypeService vehicleTypeService, IVehicleModelService vehicleModelService, IEmployeeService employeeService, IFileService fileService)
        {
            _context = context;
            _vehicleTypeService = vehicleTypeService;
            _vehicleModelService = vehicleModelService;
            _employeeService = employeeService;
            _fileServices = fileService;
        }

        public async Task<IEnumerable<BookNowDto>> GetAvailableVehiclesDetails(DateTime startDate, TimeOnly startTime, DateTime endDate, TimeOnly endTime)
        {
            var availableVehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(v => v.VehicleModel)
                .Include(v => v.Employee)
                .Where(vehicle => !vehicle.CusReservation
                    .Any(reservation =>
                        !(reservation.Reservation.EndDate < startDate ||
                          reservation.Reservation.StartDate > endDate ||
                          (reservation.Reservation.EndDate == startDate && reservation.Reservation.EndTime <= startTime) ||
                          (reservation.Reservation.StartDate == endDate && reservation.Reservation.StartTime >= endTime)
                        )
                    ))
                .ToListAsync();

            if (availableVehicles == null || !availableVehicles.Any())
            {
                throw new DataNotFoundException("No available vehicles found");
            }

            var bookNowDtos = new List<BookNowDto>();
            foreach (var vehicle in availableVehicles)
            {
                var vehicleType = await _vehicleTypeService.GetSingleVehicleType(vehicle.VehicleTypeId);
                var vehicleModel = await _vehicleModelService.GetVehicleModel(vehicle.VehicleModelId);

                var bookNowDto = new BookNowDto
                {
                    VehicleId = vehicle.Id,
                    Name = vehicleModel.Name,
                    Make = vehicleModel.VehicleMake.Name,
                    Type = vehicleType.Name,
                    Year = vehicleModel.Year,
                    Transmission = vehicle.Transmission,
                    SeatingCapacity = vehicleModel.SeatingCapacity,
                    CostPerDay = vehicle.CostPerDay,
                    Thumbnail = vehicle.Thumbnail,
                    FrontImg = vehicle.FrontImg,
                    RearImg = vehicle.RearImg,
                    DashboardImg = vehicle.DashboardImg,
                    InteriorImg = vehicle.InteriorImg,
                    Logo = vehicleModel.VehicleMake.Logo
                };
                bookNowDtos.Add(bookNowDto);
            }
            return bookNowDtos;
        }

    }
}
