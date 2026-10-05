namespace Customer.Application.Common.Contracts.Customers;

public record CustomerPaymentRequest(
    int MerchantId,
    Guid MerchantGuid,
    int CustomerId,
    Guid CustomerGuid,
    decimal Amount,
    string Currency,
    string Type,
    string Provider,
    string PhoneNumber
);
