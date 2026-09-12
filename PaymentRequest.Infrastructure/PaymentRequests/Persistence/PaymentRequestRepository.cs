namespace PaymentRequest.Infrastructure.PaymentRequests.Persistence;

internal class PaymentRequestRepository(ApplicationDbContext context) : IPaymentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<Domain.Entities.PaymentRequest> CreatePaymentRequestAsync(Application.Common.Contracts.Merchants.PaymentRequest request, CancellationToken cancellationToken = default)
    {
        var payment = request.Adapt<Domain.Entities.PaymentRequest>();
        await _context.PaymentRequests.AddAsync(payment, cancellationToken);
        return payment;
    }

    public async Task<Domain.Entities.PaymentRequest> GetPaymentByReferenceAsync(string reference, CancellationToken cancellationToken = default) =>
        await _context.PaymentRequests
            .SingleOrDefaultAsync(p => p.Reference.Equals(reference), cancellationToken);
}