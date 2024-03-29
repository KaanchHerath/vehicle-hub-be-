namespace reservation_system_be.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Year { get; set; } = string.Empty;
        public string EngineCapacity { get; set; } = string.Empty;
        public string SeatingCapacity {  get; set; } = string.Empty;
        public string Fuel { get; set; } = string.Empty;
        public int VehicleMakeId { get; set; }
        public VehicleMake VehicleMake { get; set; } = null!;
        public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public ICollection<AdditionalFeatures>? AdditionalFeatures { get; set; }


    }
}
