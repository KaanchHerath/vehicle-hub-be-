using System;
using Microsoft.Extensions.Options;
using Stripe;
using System.Threading.Tasks;

namespace reservation_system_be.Services.StripeService
{
    public class StripeService : IStripeService
    {
        private readonly StripeSettings _stripeSettings;

        public StripeService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = "sk_test_51PKzzr2NtGTfzr39MEIYe18LXbUb30cSASKsrn4tZBw8d3mCb5W4z7bJosknjPOOFDc6M874UUF6MhtcisNfWRB500cULgBKPt";
        }

        public async Task<PaymentIntent> CreatePaymentIntent(long amount, string currency)
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
            };

            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }
    }
}
