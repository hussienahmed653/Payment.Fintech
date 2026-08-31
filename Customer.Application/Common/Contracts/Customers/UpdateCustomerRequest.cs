namespace Customer.Application.Common.Contracts.Customers;

public record UpdateCustomerRequest(
    Guid Guid,
    string? ContactFirstName,
    string? ContactLastName,
    string? Email,
    string? Phone,
    string? BusinessName,
    BusinessType? BusinessType
);

