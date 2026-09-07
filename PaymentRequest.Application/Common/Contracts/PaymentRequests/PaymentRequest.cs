using PaymentRequest.Domain.Enums;

namespace PaymentRequest.Application.Common.Contracts.Merchants;

public record PaymentRequest(
    int MerchantId,
    Guid MerchantGuid,
    int CustomerId,
    Guid CustomerGuid,
    decimal Amount,
    PaymentCurrency Currency
);
