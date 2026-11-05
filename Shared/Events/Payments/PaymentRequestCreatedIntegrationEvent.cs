namespace Shared.Events.Payments;

public sealed record PaymentRequestCreatedIntegrationEvent(
    Guid CorrelationId,
    Guid EventId,
    Guid PaymentRequestId,
    Guid CustomerId,
    Guid MerchantId,
    string? CollectionMethod,
    decimal Amount,
    string Currency,
    DateTimeOffset OccurredOn
);
