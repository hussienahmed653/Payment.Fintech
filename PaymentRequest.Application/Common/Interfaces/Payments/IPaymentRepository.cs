namespace PaymentRequest.Application.Common.Interfaces.Merchants;

public interface IPaymentRepository
{
    Task<Domain.Entities.PaymentRequest> GetPaymentByReferenceAsync(string reference, CancellationToken cancellationToken = default);
    Task<Domain.Entities.PaymentRequest> CreatePaymentRequestAsync(Contracts.Merchants.PaymentRequest request, CancellationToken cancellationToken = default);
}
