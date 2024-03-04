namespace reservation_system_be.Models
{
    public class VehiclePhoto
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle? vehicle { get; set; }
        public byte[]? ImageData { get; set; }
    }
}
