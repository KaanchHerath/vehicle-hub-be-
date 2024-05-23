using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        [JsonIgnore]
        public Payment? Payment { get; set; }
        [ForeignKey("ReservationId")]
        public int ReservationId { get; set; }
        [JsonIgnore]
        public Reservation? Reservation { get; set; }

    }
}

