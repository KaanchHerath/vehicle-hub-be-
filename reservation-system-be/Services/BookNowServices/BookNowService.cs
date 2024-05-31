using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.CustomerVehicleServices
{
    public class BookNowService : IBookNowService
    {
        private readonly DataContext _context;
        private readonly IVehicleService _vehicleService;
        public BookNowService(DataContext dataContext,IVehicleService vehicleService) 
        {
            _context = dataContext;
            _vehicleService = vehicleService;
        }
        public async Task<IEnumerable<BookNowDto>> GetAllCards()
        {
            var vehicles = await _vehicleService.GetAllVehicles();
            var bookNowDtos = new List<BookNowDto>();

            foreach (var vehicle in vehicles)
            {
                var bookNowDto = new BookNowDto
                {
                    VehicleId = vehicle.Id,
                    Name = vehicle.VehicleModel.Name,
                    Make = vehicle.VehicleModel.VehicleMake.Name,
                    Type = vehicle.VehicleType.Name,
                    Year = vehicle.VehicleModel.Year,
                    Transmission = vehicle.Transmission,
                    SeatingCapacity = vehicle.VehicleModel.SeatingCapacity,
                    CostPerDay = vehicle.CostPerDay
                };
                bookNowDtos.Add(bookNowDto);
            } 
            return bookNowDtos;
        }
            
    }
}
