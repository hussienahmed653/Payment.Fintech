using Customer.Domain.Enums;

namespace Customer.Domain.Entities;

public sealed class Customer : AuditableEntity
{
    public int Id { get; set; }
    public Guid GuidId { get; set; } = Guid.CreateVersion7();
    public string ContactFirstName { get; set; } = string.Empty;
    public string ContactLastName { get; set; } = String.Empty;
    public string FullName => $"{ContactFirstName} {ContactLastName}";
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public BusinessType BusinessType { get; set; } = BusinessType.Individual;
    public string TaxId { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD";
    public CustomerStatus Status { get; set; } = CustomerStatus.Pending;
    public decimal DailyLimit { get; set; }
    public int MerchantId { get; set; }
    public int MerchantGuid { get; set; }
}

