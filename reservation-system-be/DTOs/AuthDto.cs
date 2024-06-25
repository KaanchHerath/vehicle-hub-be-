namespace reservation_system_be.DTOs
{
    public record struct AuthDto
    (
        string token,
        string id, 
        bool status
    );
    
}