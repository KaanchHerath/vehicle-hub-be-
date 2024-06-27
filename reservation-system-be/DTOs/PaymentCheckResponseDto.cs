namespace reservation_system_be.DTOs
{
    public class PaymentCheckResponseDto
    {
        public bool PaymentExists { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
