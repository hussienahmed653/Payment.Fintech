using PaymentRequest.Domain.Enums;

namespace PaymentRequest.Application.Common.Contracts.Merchants;

public record PaymentResponse
(
    int Id,
    Guid GuidId,
    string Reference,
    int MerchantId,
    Guid MerchantGuid,
    int CustomerId,
    Guid CustomerGuid,
    decimal Amount,
    string Currency,
    string Status,
    DateTime CreatedAt
);
