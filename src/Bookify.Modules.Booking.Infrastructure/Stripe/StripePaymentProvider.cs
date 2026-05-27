using Bookify.Modules.Booking.Application.Abstract;
using Bookify.Modules.Booking.Application.Abstract.Models;
using Stripe.Checkout;

namespace Bookify.Modules.Booking.Infrastructure.Stripe;

internal sealed class StripePaymentProvider(SessionService sessionService) : IPaymentProvider
{
    public async Task<PaymentSessionDto> CreateSessionAsync(Guid orderId, decimal amount, string currency,
        string successfulUrl,
        string cancelUrl, CancellationToken ct)
    {
        var options = new SessionCreateOptions
        {
            SuccessUrl = successfulUrl,
            CancelUrl = cancelUrl,
            Mode = "payment",
            PaymentMethodTypes = ["card"],
            LineItems =
            [
                new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency.ToLowerInvariant(),

                        UnitAmount = (long)(amount * 100),

                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Order #{orderId}",
                            Description = "Only for testing purposes"
                        }
                    }
                }
            ]
        };

        var session = await sessionService.CreateAsync(options, cancellationToken: ct);

        return new PaymentSessionDto
        {
            Id = session.Id,
            Url = session.Url,
        };
    }

    public async Task<PaymentSessionDto?> GetSessionStatusAsync(string sessionId, CancellationToken ct = default)
    {
        var session = await sessionService.GetAsync(sessionId, cancellationToken: ct);

        if (session is null)
        {
            return null;
        }

        return new PaymentSessionDto
        {
            Id = session.Id,
            Url = session.Url,
            PaymentStatus = session.PaymentStatus,
        };
    }
}