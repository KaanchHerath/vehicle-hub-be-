
namespace reservation_system_be.DTOs
{
    public record struct CreateVehicleInsuranceDto
    (
        int Id,
        string InsuranceNo,
        DateTime ExpiryDate,
        int VehicleId
    );
}
