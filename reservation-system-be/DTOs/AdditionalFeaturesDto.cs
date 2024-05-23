using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct AdditionalFeaturesDto
    (
        int Id,
        bool ABS,
        bool AcFront,
        bool SecuritySystem,
        bool Bluetooth,
        bool ParkingSensor,
        bool AirbagDriver,
        bool AirbagPassenger,
        bool AirbagSide,
        bool FogLights,
        bool NavigationSystem,
        bool Sunroof,
        bool TintedGlass,
        bool PowerWindow,
        bool RearWindowWiper,
        bool AlloyWheels,
        bool ElectricMirrors,
        bool AutomaticHeadlights,
        bool KeylessEntry,
        VehicleModelDto VehicleModel
    );
}
