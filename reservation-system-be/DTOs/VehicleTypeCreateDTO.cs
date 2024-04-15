namespace reservation_system_be.DTOs
{
    public record struct VehicleTypeCreateDTO(
        string Name,
        float DepositAmount
    );
}
