using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace reservation_system_be.Models
{
    public class VehicleInsurance
    {
        public int Id { get; set; }
        public string InsuranceNo { get; set; } = string.Empty;
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime? ExpiryDate { get; set; }

        [ForeignKey("VehicleId")]
        public int VehicleId { get; set; }
        [JsonIgnore]
        public Vehicle? Vehicle { get; set; }
        public bool Status
            {
                get
                {
                    return DateTime.Now <= ExpiryDate;
                }
            }
    }
}
