using PaymentRequest.Domain.Entities;

namespace PaymentRequest.Application.Common.Interfaces.Payments;

public interface IPaymentTransactionRepository
{
    Task<PaymentTransaction> GetByPaymentRequestGuidAsync(Guid paymentRequestGuid, CancellationToken cancellationToken = default!);
    Task AddAsync(PaymentTransaction paymentTransaction, CancellationToken cancellationToken = default!);
}
