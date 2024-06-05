using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehiclePhoto
    {
        public int Id { get; set; }
        public byte[]? ImageData { get; set; }
        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        [JsonIgnore]
        public Vehicle Vehicle { get; set; } = null!;
    }
}
