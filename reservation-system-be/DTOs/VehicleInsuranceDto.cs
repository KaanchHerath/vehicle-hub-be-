using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct VehicleInsuranceDto
    (
        int Id,
        string InsuranceNo,
        DateTime ExpiryDate,
        VehicleDto Vehicle,
        bool Status
    );
}
