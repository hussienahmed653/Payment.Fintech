namespace Shared.Events.Payments;

public sealed record PaymentRequestSucceededIntegrationEvent(
    Guid EventId,
    Guid PaymentRequestId,
    Guid CustomerId,
    Guid MerchantId,
    decimal Amount,
    string Currency,
    string PaymentReference,
    DateTimeOffset OccurredOn
);
