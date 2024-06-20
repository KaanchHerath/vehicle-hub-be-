using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public float Amount { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public Payment? Payment { get; set; }
        [ForeignKey("CustomerReservationId")]
        public int CustomerReservationId { get; set; }
        [JsonIgnore]
        public CustomerReservation? CustomerReservation { get; set; }

    }
}

