using reservation_system_be.Models;
using System;

namespace reservation_system_be.DTOs
{
    public record struct BillingDetailsDTO(
        int Id,
        float Amount,
        DateTime DateCreated,
        Status ReservationStatus,
        int CustomerId,
        string CusName,
        string RegistrationNumber
    );
}
