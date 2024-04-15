using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehicleType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float DepositAmount { get; set; }
        [JsonIgnore]
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
