using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class CustomerReservation
    {
        public int Id { get; set; }

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        [JsonIgnore]
        public Customer? Customer { get; set; }

        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        [JsonIgnore]
        public Vehicle? Vehicle { get; set; }

        [ForeignKey("ReservationId")]
        public int ReservationId { get; set; }
        [JsonIgnore]
        public Reservation? Reservation { get; set; }
        [JsonIgnore]
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        [JsonIgnore]
        public VehicleLog? VehicleLog { get; set; }
        [JsonIgnore]
        public Feedback? Feedback { get; set; }
    }
}
