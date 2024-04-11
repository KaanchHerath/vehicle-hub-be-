namespace reservation_system_be.Models
{
    public class CustomerTelephone
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
