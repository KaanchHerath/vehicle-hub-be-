﻿using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct OngoingRentalDto
    (
        int CustomerReservationId,
        string ModelName,
        string Make,
        DateTime StartDate,
        DateTime EndDate,
        Status Status
    );
}
