using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct CreateVehicleModelDto(
        VehicleModel VehicleModel,
        string[] AdditionalFeatures
    );
}