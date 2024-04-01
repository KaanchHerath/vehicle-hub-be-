namespace reservation_system_be.Models
{
    public class AdditionalFeatures
    { 
        public int Id { get; set; }
        public string Features { get; set; } = string.Empty;
        public int VehicleModelId { get; set; }
        public VehicleModel VehicleModel { get; set; } = null!;

    }
}
