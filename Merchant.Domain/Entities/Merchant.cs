using Merchant.Domain.Enums;

namespace Merchant.Domain.Entities;

public sealed class Merchant : AuditableEntity
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
    public MerchantStatus Status { get; set; } = MerchantStatus.Pending;
    public decimal DailyLimit { get; set; }
}

