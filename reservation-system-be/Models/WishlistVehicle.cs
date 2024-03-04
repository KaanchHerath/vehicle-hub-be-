using Azure;
using Microsoft.Extensions.Hosting;

namespace reservation_system_be.Models
{
    public class WishlistVehicle
    {
        public int Id { get; set; }
        public int CustomerWishlistId { get; set; }
        public CustomerWishlist? customerWishlist { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
