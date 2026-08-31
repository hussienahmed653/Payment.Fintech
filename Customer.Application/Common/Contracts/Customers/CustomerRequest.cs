namespace Customer.Application.Common.Contracts.Customers;

public record CustomerRequest(
    string ContactFirstName,
    string ContactLastName,
    string Email,
    string Phone,
    string BusinessName,
    BusinessType BusinessType,
    string TaxId,
    string Currency,
    CustomerStatus Status,
    decimal DailyLimit
);
