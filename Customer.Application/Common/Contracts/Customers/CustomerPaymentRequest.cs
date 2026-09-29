namespace Customer.Application.Common.Contracts.Customers;

public record CustomerPaymentRequest(
    int MerchantId,
    Guid MerchantGuid,
    int CustomerId,
    Guid CustomerGuid,
    decimal Amount,
    Currency Currency,
    string Type,
    string ProviderNetwork,
    string PhoneNumber
);
