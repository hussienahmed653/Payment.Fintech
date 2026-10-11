namespace Shared.Sections;

public record CustomerRequestSection(
    string? ContactFirstName = null,
    string? ContactLastName = null,
    string? Email = null,
    string? Phone = null,
    string? BusinessName = null,
    BusinessType? BusinessType = null
);
