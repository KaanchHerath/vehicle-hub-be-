namespace reservation_system_be.Models
{
    public class ResetPasswordRequest
    {
        public string Otp { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
