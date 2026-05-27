using Bookify.Modules.Booking.Application.Abstract.Models;

namespace Bookify.Modules.Booking.Application.Abstract;

public interface IPaymentProvider
{
    public Task<PaymentSessionDto> CreateSessionAsync(Guid orderId, decimal amount, string currency,
        string successfulUrl,
        string cancelUrl, CancellationToken ct = default);

    public Task<PaymentSessionDto?> GetSessionStatusAsync(string sessionId, CancellationToken ct = default);
}