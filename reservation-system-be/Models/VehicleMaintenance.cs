namespace reservation_system_be.Models
{
    public class VehicleMaintenance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public int MaintenanceTypeId { get; set; }
        public MaintenanceType MaintenanceType { get; set; } = null!;
    }
}
