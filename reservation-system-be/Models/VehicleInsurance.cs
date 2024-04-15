namespace reservation_system_be.Models
{
    public class VehicleInsurance
    {
        public int Id { get; set; }
        public string InsuranceNo { get; set; } = string.Empty;
        public DateTime? ExpiryDate { get; set; }
        public int VehicleId { get; set; }
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
