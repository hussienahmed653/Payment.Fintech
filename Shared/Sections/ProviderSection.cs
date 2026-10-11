namespace Shared.Sections;

public record ProviderSection(
    Guid DepositId,
    PayerDto Payer,
    string? PreAuthorisationCode,
    string? ClientReferenceId,
    string? CustomerMessage
);

public record PayerDto(
    PayerType Type,
    AccountDetailsDto AccountDetails
);
public record AccountDetailsDto(
    string Provider,
    string PhoneNumber
);