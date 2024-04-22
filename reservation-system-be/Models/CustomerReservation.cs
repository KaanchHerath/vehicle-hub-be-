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
        
        [JsonIgnore]
        public Reservation? Reservation { get; set; }
    }
}
