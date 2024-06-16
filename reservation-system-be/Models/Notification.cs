using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Generated_DateTime { get; set; }
        [ForeignKey("CustomerReservationId")]
        public int? CustomerReservationId { get; set; } = null;
        [JsonIgnore]
        public CustomerReservation? CustomerReservation { get; set; }
        [ForeignKey("VehicleInsuranceID")]
        public int? VehicleInsuranceID { get; set; } = null;
        [JsonIgnore]
        public VehicleInsurance? VehicleInsurance { get; set; }
        [ForeignKey("VehicleMaintenanceId")]
        public int? VehicleMaintenanceId { get; set; } = null;
        [JsonIgnore]
        public VehicleMaintenance? VehicleMaintenance { get; set; }

    }
}
