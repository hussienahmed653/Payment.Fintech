using PaymentRequest.Domain.Entities;

namespace PaymentRequest.Application.Common.Interfaces.Payments;

public interface IPaymentTransactionRepository
{
    Task<PaymentTransaction> GetByIdemPotencyIdAsync(string idemPotencyId, CancellationToken cancellationToken = default!);
    Task AddAsync(PaymentTransaction paymentTransaction, CancellationToken cancellationToken = default!);
}
