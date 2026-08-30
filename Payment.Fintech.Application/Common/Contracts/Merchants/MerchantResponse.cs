namespace Payment.Fintech.Application.Common.Contracts.Merchants;

public record MerchantResponse
(
    Guid GuidId,
    string ContactFirstName,
    string ContactLastName,
    string FullName,
    string Email,
    string Phone,
    string BusinessName,
    string BusinessType,
    string TaxId,
    string Currency,
    string Status,
    decimal DailyLimit
);
