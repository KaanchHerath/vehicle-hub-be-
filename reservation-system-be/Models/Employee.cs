using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string NIC { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        [ForeignKey("ReservationId")]
        public int ReservationId { get; set; }
        [JsonIgnore]
        public Reservation? Reservation { get; set; }
        [JsonIgnore]
        public ICollection<Vehicle>? Vehicles { get; set; }
        [JsonIgnore]
        public ICollection<EmployeeTelephone>? employeeTelephones { get; set; }
        
    }
}
