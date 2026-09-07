using PaymentRequest.Application.Common.Contracts.Merchants;
using PaymentRequest.Application.Common.Interfaces.Merchants;

namespace PaymentRequest.Infrastructure.Merchants.Persistence;

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
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.Reference.Equals(reference), cancellationToken);

    public async Task<bool> ReferenceIsExistsAsync(string reference, CancellationToken cancellationToken = default) =>
        await _context.PaymentRequests.AnyAsync(x => x.Reference.Equals(reference), cancellationToken);
}