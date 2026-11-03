namespace Shared.Events.Payments;

public sealed record PaymentRequestCreatedIntegrationEvent(
    Guid EventId,
    Guid PaymentRequestId,
    Guid CustomerId,
    Guid MerchantId,
    decimal Amount,
    string Currency,
    DateTimeOffset OccurredOn
);
