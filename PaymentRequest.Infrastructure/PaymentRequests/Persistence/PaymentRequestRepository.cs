using PaymentRequest.Application.Common.Contracts.PaymentRequests;
using PaymentRequest.Application.Common.Interfaces.PaymentRequests;
using Shared.Sections;

namespace PaymentRequest.Infrastructure.PaymentRequests.Persistence;

internal class PaymentRequestRepository(ApplicationDbContext context) : IPaymentRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<PaymentResponse> CreatePaymentRequestAsync(PaymentRequestSection request, CancellationToken cancellationToken = default)
    {
        var payment = request.Adapt<Domain.Entities.PaymentRequest>();
        await _context.PaymentRequests.AddAsync(payment, cancellationToken);
        return payment.Adapt<PaymentResponse>();
    }

    public async Task<Domain.Entities.PaymentRequest> GetPaymentByReferenceAsync(string reference, CancellationToken cancellationToken = default) =>
        await _context.PaymentRequests
            .SingleOrDefaultAsync(p => p.Reference.Equals(reference), cancellationToken);

    public async Task UpdateStatus(Guid paymentId, PaymentStatus status)
    {
        var payment = await _context.PaymentRequests.SingleOrDefaultAsync(p => p.GuidId == paymentId);
        payment?.Status = status;
    }
}