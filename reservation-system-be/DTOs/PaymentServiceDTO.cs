using reservation_system_be.Models;
using System;

namespace reservation_system_be.DTOs
{
    public class PaymentServiceDTO
    {
        private Status status;

        public PaymentServiceDTO(int id, string paymentStatus, string paymentMethod, DateTime paymentDate, DateTime paymentTime, int invoiceId, Status status)
        {
            Id = id;
            PaymentStatus = paymentStatus;
            PaymentMethod = paymentMethod;
            PaymentDate = paymentDate;
            PaymentTime = paymentTime;
            InvoiceId = invoiceId;
            this.status = status;
        }

        public int Id { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime PaymentTime { get; set; }
        public int InvoiceId { get; set; }
        public Status ReservationStatus { get; set; }
    }
}
