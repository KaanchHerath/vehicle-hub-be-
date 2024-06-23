using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct VehicleModelDto(
        CreateVehicleModelDto vehicleModel,
        CreateAdditionalFeaturesDto additionalFeatures
    );
}