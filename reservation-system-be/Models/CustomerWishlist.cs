namespace reservation_system_be.Models
{
    public class CustomerWishlist
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int WishlistId { get; set; } 
        public Wishlist? Wishlist { get; set;}
        public List<WishlistVehicle>? WishlistVehicles { get; set;}
        
    }
}
