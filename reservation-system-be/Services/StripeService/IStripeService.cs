using System;
using Stripe;

namespace reservation_system_be.Services.StripeService
{
    public interface IStripeService
    {
        Task<PaymentIntent> CreatePaymentIntent(long amount, string currency);
    }
}
