namespace Payment.Fintech.Application.Common.Contracts.Merchants;

public record MerchantRequest(
    string ContactFirstName,
    string ContactLastName,
    string Email,
    string Phone,
    string BusinessName,
    BusinessType BusinessType,
    string TaxId,
    string Currency,
    MerchantStatus Status,
    decimal DailyLimit
);
