using Shared.Sections;

namespace PaymentRequest.Application.Common.Interfaces.PaymentRequests;

public interface IPaymentRepository
{
    Task<Domain.Entities.PaymentRequest> GetPaymentByReferenceAsync(string reference, CancellationToken cancellationToken = default);
    Task<PaymentResponse> CreatePaymentRequestAsync(PaymentRequestSection request, CancellationToken cancellationToken = default);
    Task UpdateStatus(Guid paymentId, PaymentStatus status);
}
