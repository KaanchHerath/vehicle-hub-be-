using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string DrivingLicenseNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Status { get; set; } = true;
        public int ContactNo { get; set; } 
        public string Address { get; set; } = string.Empty;
        public int ContactNo { get; set; }
        public string Password { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Wishlist>? Wishlist { get; set; }
        [JsonIgnore]
        public CustomerReservation? CusReservation { get; set; }
    }
}
