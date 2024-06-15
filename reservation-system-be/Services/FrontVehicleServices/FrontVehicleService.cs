using reservation_system_be.Data;
using reservation_system_be.DTOs;
using reservation_system_be.Models;
using reservation_system_be.Services.AdditionalFeaturesServices;
using reservation_system_be.Services.VehicleModelServices;
using reservation_system_be.Services.VehicleServices;

namespace reservation_system_be.Services.CustomerVehicleServices
{
    public class FrontVehicleService : IFrontVehicleService
    {
        private readonly DataContext _context;
        private readonly IVehicleService _vehicleService;
        public FrontVehicleService(DataContext dataContext, IVehicleService vehicleService)
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
                    CostPerDay = vehicle.CostPerDay,
                    Thumbnail = vehicle.Thumbnail,
                    FrontImg = vehicle.FrontImg,
                    RearImg = vehicle.RearImg,
                    DashboardImg = vehicle.DashboardImg,
                    InteriorImg = vehicle.InteriorImg,
                    Logo = vehicle.VehicleModel.VehicleMake.Logo
                };
                bookNowDtos.Add(bookNowDto);
            }
            return bookNowDtos;
        }

        public async Task<VehicleImagesDto> GetImages(int id)
        {
            var vehicle = await _vehicleService.GetVehicle(id);
            var vehicleImages = new VehicleImagesDto
            {
                FrontImg = vehicle.FrontImg,
                RearImg = vehicle.RearImg,
                DashboardImg = vehicle.DashboardImg,
                InteriorImg = vehicle.InteriorImg
            };
            return vehicleImages;
        }

        public async Task<CreateAdditionalFeaturesDto> GetAdditionalFeatures(int id) //vehicle id
        {
            var vehicle = await _vehicleService.GetVehicle(id); 
            var vehicleModelId = vehicle.VehicleModel.Id;
            var additionalFeatures = _context.AdditionalFeatures.FirstOrDefault(x => x.VehicleModelId == vehicleModelId);
            if (additionalFeatures == null)
            {
                throw new Exception("Additional features not found");
            }
            var createAdditionalFeaturesDto = new CreateAdditionalFeaturesDto
            {
                ABS = additionalFeatures.ABS,
                AcFront = additionalFeatures.AcFront,
                SecuritySystem = additionalFeatures.SecuritySystem,
                Bluetooth = additionalFeatures.Bluetooth,
                ParkingSensor = additionalFeatures.ParkingSensor,
                AirbagDriver = additionalFeatures.AirbagDriver,
                AirbagPassenger = additionalFeatures.AirbagPassenger,
                AirbagSide = additionalFeatures.AirbagSide,
                FogLights = additionalFeatures.FogLights,
                NavigationSystem = additionalFeatures.NavigationSystem,
                Sunroof = additionalFeatures.Sunroof,
                TintedGlass = additionalFeatures.TintedGlass,
                PowerWindow = additionalFeatures.PowerWindow,
                RearWindowWiper = additionalFeatures.RearWindowWiper,
                AlloyWheels = additionalFeatures.AlloyWheels,
                ElectricMirrors = additionalFeatures.ElectricMirrors,
                AutomaticHeadlights = additionalFeatures.AutomaticHeadlights,
                KeylessEntry = additionalFeatures.KeylessEntry
            };
            return createAdditionalFeaturesDto;
        }
    }
}
