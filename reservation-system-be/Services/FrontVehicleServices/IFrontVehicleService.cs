using reservation_system_be.DTOs;

namespace reservation_system_be.Services.CustomerVehicleServices
{
    public interface IFrontVehicleService
    {
        Task<IEnumerable<BookNowDto>> GetAllCards();
        Task<VehicleImagesDto> GetImages(int id);
        Task<CreateAdditionalFeaturesDto> GetAdditionalFeatures(int id);
    }
}
