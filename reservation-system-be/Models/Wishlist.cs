namespace reservation_system_be.Models
{
    public class Wishlist
    {
        public int Id { get; set; }
        public int ItemCount { get; set; }
       public CustomerWishlist? customerWishlist { get; set; }

    }
}
