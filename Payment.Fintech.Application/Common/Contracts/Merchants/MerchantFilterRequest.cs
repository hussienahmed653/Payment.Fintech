namespace Payment.Fintech.Application.Common.Contracts.Merchants;

public record MerchantFilterRequest(
    string? ContactFirstName,
    string? ContactLastName,
    string? Phone,
    string? BusinessName,
    string? BusinessType,
    string? Status,
    string? Currency
);
