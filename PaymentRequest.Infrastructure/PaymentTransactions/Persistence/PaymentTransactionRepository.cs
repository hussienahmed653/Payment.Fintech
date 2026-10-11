namespace PaymentRequest.Infrastructure.PaymentTransactions.Persistence;

internal class PaymentTransactionRepository(ApplicationDbContext context) : IPaymentTransactionRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(PaymentTransaction paymentTransaction, CancellationToken cancellationToken = default!) =>
        await _context.AddAsync(paymentTransaction, cancellationToken);

    public async Task<PaymentTransaction> GetByPaymentRequestGuidAsync(Guid paymentRequestGuid, CancellationToken cancellationToken = default) =>
        await _context.PaymentTransactions
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PaymentRequestGuid == paymentRequestGuid, cancellationToken) ?? default!;
    public async Task UpdateAsync(Guid guid, PaymentStatus status, CancellationToken cancellationToken = default)
    {
        var paymentTransaction = await _context.PaymentTransactions.SingleOrDefaultAsync(p => p.GuidId == guid, cancellationToken);
        paymentTransaction?.Status = status;
    }
}
