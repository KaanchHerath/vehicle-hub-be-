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
        public bool Status { get; set; } = true;
        public int ContactNo { get; set; }
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Vehicle>? Vehicles { get; set; }
        [JsonIgnore]
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
