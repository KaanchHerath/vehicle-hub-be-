namespace reservation_system_be.DTOs
{
    public record struct BookingConfirmationDto
    (
        int CustomerReservationId,
        string Make,
        string ModelName,
        DateTime StartDate,
        DateTime EndDate,
        TimeOnly StartTime,
        TimeOnly EndTime,
        float Deposit,
        float ExtraKMCost,
        float Penalty,
        float RentalCost,
        float Amount
    );
}
