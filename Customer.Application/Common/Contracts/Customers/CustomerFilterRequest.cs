namespace Customer.Application.Common.Contracts.Customers;

public record CustomerFilterRequest(
    string? ContactFirstName,
    string? ContactLastName,
    string? Phone,
    string? BusinessName,
    string? BusinessType,
    string? Status,
    string? Currency
);
