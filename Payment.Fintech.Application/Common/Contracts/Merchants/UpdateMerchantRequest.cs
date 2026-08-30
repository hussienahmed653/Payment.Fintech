namespace Payment.Fintech.Application.Common.Contracts.Merchants;

public record UpdateMerchantRequest(
    string? ContactFirstName,
    string? ContactLastName,
    string? Email,
    string? Phone,
    string? BusinessName,
    BusinessType? BusinessType
);

