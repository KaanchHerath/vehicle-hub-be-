namespace reservation_system_be.DTOs
{
    public record struct CustomerHoverDto
    (
        int Id,
        string Name,
        string Email,
        int Phone
    );
}
