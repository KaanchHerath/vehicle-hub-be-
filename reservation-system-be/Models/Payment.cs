using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentTime { get; set; }
        [ForeignKey("InvoiceId")]
        public int InvoiceId { get; set; }
        [JsonIgnore]
        public Invoice? Invoice { get; set; }

    }
}
