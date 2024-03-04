namespace reservation_system_be.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NIC { get; set; } = string.Empty;
        public string DrivingLicenseNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string TelephoneNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public CustomerWishlist? CustomerWishlist { get; set; }
        public int WishlistId { get; set; }
        public Wishlist? Wishlist { get; set; }
        public ICollection<CustomerReservation> CustomerReservations { get; } = new List<CustomerReservation>();
    }
}
