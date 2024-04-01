namespace reservation_system_be.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentTime { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

    }
}
