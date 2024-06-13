using System;
using reservation_system_be.Models;

namespace reservation_system_be.DTOs
{
    public record struct PaymentServiceDTO(
        string PaymentStatus,
        string PaymentMethod,
        DateTime PaymentDate,
        DateTime PaymentTime,
        int InvoiceId,
        Status ReservationStatus
    );
}
