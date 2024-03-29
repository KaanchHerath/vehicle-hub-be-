namespace reservation_system_be.Models
{
    public class VehicleMaintenance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<MaintenanceType> MaintenanceTypes { get; set; } = new List<MaintenanceType>();
        public ICollection<Vehicle> Vehicle { get; } = new List<Vehicle>();
    }
}
