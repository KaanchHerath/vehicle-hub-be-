namespace reservation_system_be.DTOs
{
    public record struct MaintenanceTypeCreateDTO(
      string Name,
      int MaintenanceId
  );
}