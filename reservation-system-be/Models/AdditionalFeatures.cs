using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class AdditionalFeatures
    { 
        public int Id { get; set; }
        public string Features { get; set; } = string.Empty;
        [ForeignKey("VehicleModelId")]
        public int VehicleModelId { get; set; }
        [JsonIgnore]
        public VehicleModel VehicleModel { get; set; } = null!;

    }
}
