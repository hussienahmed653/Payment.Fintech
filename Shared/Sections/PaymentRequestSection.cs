namespace Shared.Sections;

public record PaymentRequestSection(
    int MerchantId,
    Guid MerchantGuid,
    int? CustomerId,
    Guid? CustomerGuid,
    decimal Amount,
    Currency Currency
);
