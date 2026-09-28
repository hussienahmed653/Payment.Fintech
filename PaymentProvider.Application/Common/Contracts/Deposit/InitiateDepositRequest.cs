using PaymentProvider.Domain.Enums;
using System.Text.Json.Serialization;

namespace PaymentProvider.Application.Common.Contracts.Deposit;

public class InitiateDepositRequest
{
    public Guid DepositId { get; set; } = Guid.CreateVersion7();
    public string Amount { get; set; }
    public string Currency { get; set; }
    public PayerDto Payer { get; set; }
    public string? preAuthorisationCode { get; set; }
    public string? ClientReferenceId { get; set; }
    public string? CustomerMessage { get; set; }
};

public record PayerDto(
    PaymentProviderType Type,
    AccountDetailsDto AccountDetails
);
public record AccountDetailsDto(
    string Provider,
    string PhoneNumber
);