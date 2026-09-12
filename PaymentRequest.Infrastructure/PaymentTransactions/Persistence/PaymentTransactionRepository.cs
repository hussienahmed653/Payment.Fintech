namespace PaymentRequest.Infrastructure.PaymentTransactions.Persistence;

internal class PaymentTransactionRepository(ApplicationDbContext context) : IPaymentTransactionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(PaymentTransaction paymentTransaction, CancellationToken cancellationToken = default!) =>
        await _context.AddAsync(paymentTransaction, cancellationToken);

    public async Task<PaymentTransaction> GetByIdemPotencyIdAsync(string idemPotencyId, CancellationToken cancellationToken = default) =>
        await _context.PaymentTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.IdemPotency.Equals(idemPotencyId), cancellationToken) ?? default!;
}
