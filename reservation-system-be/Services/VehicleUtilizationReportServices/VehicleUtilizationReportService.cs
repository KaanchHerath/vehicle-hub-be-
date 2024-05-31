using System;
using reservation_system_be.Data;
using reservation_system_be.Models;
using Microsoft.EntityFrameworkCore;
using reservation_system_be.DTOs;

namespace reservation_system_be.Services.VehicleUtilizationReportServices
{
    public class VehicleUtilizationReportService : IVehicleUtilizationReportService
    {
        private readonly DataContext _context;

        public VehicleUtilizationReportService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleUtilizationReportDTO>> GetAllVehicleUtilization()
        {
            try
            {
                return await _context.CustomerReservations
                 .Include(cr => cr.Reservation)
                 .Include(cr => cr.Vehicle)
                 .Select(cr => new VehicleUtilizationReportDTO
                 {
                     vehicleNo = cr.Vehicle.RegistrationNumber,
                     startDate = cr.Reservation.StartDate,
                     endDate = cr.Reservation.EndDate,
                     mileage = cr.Vehicle.Mileage,
                     reservationId = cr.ReservationId
                 })
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting vehicle utilization", ex);
            }
        }
    }
}
// } @bikz
