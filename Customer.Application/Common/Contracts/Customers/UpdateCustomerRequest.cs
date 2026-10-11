namespace Customer.Application.Common.Contracts.Customers;

public record UpdateCustomerRequest(
    Guid CustomerGuid,
    string? ContactFirstName,
    string? ContactLastName,
    string? Email,
    string? Phone,
    string? BusinessName,
    BusinessType? BusinessType
);

