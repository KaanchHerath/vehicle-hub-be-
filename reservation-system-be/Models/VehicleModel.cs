using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Year { get; set; } 
        public int EngineCapacity { get; set; }
        public int SeatingCapacity {  get; set; }
        public string Fuel { get; set; } = string.Empty;
        [ForeignKey("VehicleMakeId")]
        public int VehicleMakeId { get; set; }
        public VehicleMake? VehicleMake { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public AdditionalFeatures? AdditionalFeatures { get; set; }
    }
}
