namespace PaymentRequest.Application.Common.Interfaces.Payments;

public interface IPaymentGatewayRepository
{
    Task<PaymentGatewayResponse> ProcessPaymentAsync(string reference, decimal amount, string currency, CancellationToken cancellationToken = default);
}
