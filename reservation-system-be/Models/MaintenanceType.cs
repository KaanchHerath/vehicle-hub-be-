namespace reservation_system_be.Models
{
    public class MaintenanceType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<VehicleMaintenance> VehicleMaintenances { get; set;} = new List<VehicleMaintenance>();
    }
}
