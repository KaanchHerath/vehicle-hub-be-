using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehicleMaintenance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        [JsonIgnore]
        public Vehicle? Vehicle { get; set; }
    }
}
