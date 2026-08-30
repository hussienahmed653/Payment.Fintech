using Payment.Fintech.Domain.Enums;

namespace Payment.Fintech.Application.Common.Contracts.Merchants;

public record UpdateMerchantRequest(
    Guid Guid,
    string? ContactFirstName,
    string? ContactLastName,
    string? Email,
    string? Phone,
    string? BusinessName,
    BusinessType? BusinessType
);

