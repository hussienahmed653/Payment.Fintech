using PaymentRequest.Domain.Enums;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PaymentRequest.Domain.Entities;

public sealed class PaymentRequest : AuditableEntity
{
    public int Id { get; set; }
    public Guid GuidId { get; set; } = Guid.CreateVersion7();
    public string Reference { get; set; }
    public int MerchantId { get; set; }
    public Guid MerchantGuid { get; set; }
    public int? CustomerId { get; set; } = null;
    public Guid? CustomerGuid { get; set; } = null;
    public decimal Amount { get; set; }
    public PaymentCurrency Currency { get; private set; }
    public PaymentStatus Status { get; set; }
    public PaymentRequest()
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyyMMdd");
        var randomNumber = Guid.CreateVersion7().ToString("N")[..8].ToUpper();
        Reference = $"PAY-{date}-{randomNumber}";
    }
}

