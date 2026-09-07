namespace PaymentRequest.Application.Common.Contracts.Merchants;

public record PaymentFilterRequest(
    string? ContactFirstName,
    string? ContactLastName,
    string? Phone,
    string? BusinessName,
    string? BusinessType,
    string? Status,
    string? Currency
);
