namespace reservation_system_be.DTOs
{
    public record struct CustomerAuthDTO
    (
        string Email, 
        string Password
    );
}
