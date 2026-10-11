namespace Shared.Sections;

public record TransactionSection(
    string Reference,
    string IdempotencyId
);
