using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehicleLog
    {
        public int Id { get; set; }
        public int EndMileage { get; set; }
        public int Penalty { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ExtraDays { get; set; }
        public int ExtraKM { get; set; }
        [ForeignKey("CustomerReservationId")]
        public int CustomerReservationId { get; set; }
        [JsonIgnore]
        public CustomerReservation? CustomerReservation { get; set; }
    }
}
