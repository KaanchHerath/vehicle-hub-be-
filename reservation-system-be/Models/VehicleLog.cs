using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehicleLog
    {
        public int Id { get; set; }
        public int EndMileage { get; set; }
        public string Penalty { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ExtraDays { get; set; }

        [ForeignKey("ReservationId")]
        public int ReservationId { get; set; }
        [JsonIgnore]
        public Reservation? Reservation { get; set; }
    }
}
