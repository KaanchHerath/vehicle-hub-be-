namespace reservation_system_be.DTOs
{
    public record struct VehicleHoverDto
   (
        int Id,
        string RegistrationNumber,
        string Model,
        string Type,
        int Year,
        string Thumbnail
   );
}
