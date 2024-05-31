using System.Drawing;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public byte[]? LogoImage { get; set; }
        [JsonIgnore]
        public ICollection<VehicleModel>? VehicleModels { get; set; } = new List<VehicleModel>();

    }
}
