namespace reservation_system_be.Models
{
    public class Wishlist
    {
        public int VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public CustomerWishlist? customerWishlist { get; set; }

    }
}


