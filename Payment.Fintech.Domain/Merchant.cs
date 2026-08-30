namespace Payment.Fintech.Domain;

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

    public void UpdateProfile(
        string? contactFirstName,
        string? contactLastName,
        string? email,
        string? phone,
        string? businessName,
        BusinessType? businessType)
    {
        if (!string.IsNullOrWhiteSpace(contactFirstName))
            ContactFirstName = contactFirstName;

        if (!string.IsNullOrWhiteSpace(contactLastName))
            ContactLastName = contactLastName;

        if (!string.IsNullOrWhiteSpace(email))
            Email = email;

        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone;

        if (!string.IsNullOrWhiteSpace(businessName))
            BusinessName = businessName;

        if (!string.IsNullOrWhiteSpace(businessType.ToString()))
            BusinessType = (BusinessType)businessType!;
    }
}   

public enum MerchantStatus
{
    Pending,
    Active,
    Suspended,
    Inactive
}

public enum BusinessType
{
    Individual,
    SoleProprietorship,
    LLC,
    Corporation,
    NonProfit
}


