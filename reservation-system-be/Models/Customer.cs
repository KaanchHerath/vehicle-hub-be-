namespace reservation_system_be.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string DrivingLicenseNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICollection<CustomerTelephone>? CustomerTelephones { get; set; }
        public ICollection<Wishlist>? Wishlist { get; set; }
        public CustomerReservation? CusReservation { get; set; }
    }
}
