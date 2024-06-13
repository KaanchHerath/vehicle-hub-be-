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
        public int CustomerReservationId { get; set; }
        [JsonIgnore]
        public CustomerReservation? CustomerReservation { get; set; }

    }
}
