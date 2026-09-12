using PaymentRequest.Application.Common.Contracts.PaymentRequests;

namespace PaymentRequest.Infrastructure.PaymentRequests.Persistence;

public class PaymentGatewayRepository : IPaymentGatewayRepository
{
    public async Task<PaymentGatewayResponse> ProcessPaymentAsync(string reference, decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        await Task.Delay(500);

        var fakeTransactionId = $"TXN-DEMO-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

        return new PaymentGatewayResponse(
            true,
            fakeTransactionId,
            null
        );
    }
}
