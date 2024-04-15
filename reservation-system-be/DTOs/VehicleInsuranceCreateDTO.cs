namespace reservation_system_be.DTOs
{
    public record struct VehicleInsuranceCreateDTO
    {
        string InsuranceNo,
        DateTime ExpiryDate,
        int VehicleId,
        readonly bool Status
    }
}
