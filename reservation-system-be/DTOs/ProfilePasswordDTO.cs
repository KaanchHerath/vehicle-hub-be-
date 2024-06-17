namespace reservation_system_be.DTOs
{
    public class ProfilePasswordDTO
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
