using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class AdditionalFeatures
    { 
        public int Id { get; set; }
        public bool ABS { get; set; } = false;
        public bool AcFront { get; set; }= false;
        public bool SecuritySystem { get; set; } = false;
        public bool Bluetooth { get; set; } = false;
        public bool ParkingSensor { get; set; } = false;
        public bool AirbagDriver { get; set; } = false;
        public bool AirbagPassenger { get; set; } = false;
        public bool AirbagSide { get; set; } = false;
        public bool FogLights { get; set; } = false;
        public bool NavigationSystem { get; set; } = false;
        public bool Sunroof { get; set; } = false;
        public bool TintedGlass { get; set; } = false;
        public bool PowerWindow { get; set; } = false;
        public bool RearWindowWiper { get; set; } = false;
        public bool AlloyWheels { get; set; } = false;
        public bool ElectricMirrors { get; set; } = false;
        public bool AutomaticHeadlights { get; set; } = false;
        public bool KeylessEntry { get; set; } = false;

        [ForeignKey("VehicleModelId")]
        public int VehicleModelId { get; set; }
        [JsonIgnore]
        public VehicleModel VehicleModel { get; set; } = null!;

    }
}
