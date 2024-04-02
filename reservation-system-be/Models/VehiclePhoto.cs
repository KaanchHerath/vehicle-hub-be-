namespace reservation_system_be.Models
{
    public class VehiclePhoto
    {
        public int Id { get; set; }
        public byte[]? ImageData { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
    }
}
