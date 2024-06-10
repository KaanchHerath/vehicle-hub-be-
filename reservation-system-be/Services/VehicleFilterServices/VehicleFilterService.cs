using reservation_system_be.Data;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace reservation_system_be.Services.VehicleFilterServices
{
    public class VehicleFilterService : IVehicleFilterService
    {
        private readonly DataContext _context;

        public VehicleFilterService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleResponse>> GetvailableVehiclesDetails(DateTime startDate, TimeOnly startTime, DateTime endDate, TimeOnly endTime)
        {
            DateTime startDateTime = startDate.Date + startTime.ToTimeSpan();
            DateTime endDateTime = endDate.Date + endTime.ToTimeSpan();

            var availableVehicles = await _context.Vehicles
                .Where(vehicle => !vehicle.CusReservation
                    .Any(reservation => !(reservation.Reservation.StartDate >= endDateTime ||
                                          reservation.Reservation.EndDate <= startDateTime)))
                .Select(vehicle => new VehicleResponse
                {
                    Vehicle = vehicle,
                    vehicleMake = vehicle.VehicleModel.VehicleMake,
                    vehicleModel = vehicle.VehicleModel,
                    vehicleType = vehicle.VehicleType,
                    vehiclePhotos = vehicle.VehiclePhoto
                })
                .ToListAsync();

            return availableVehicles;
        }
    }
}
