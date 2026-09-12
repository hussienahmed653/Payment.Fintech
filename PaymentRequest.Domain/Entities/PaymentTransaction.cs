using PaymentRequest.Domain.Enums;

namespace PaymentRequest.Domain.Entities;

public class PaymentTransaction : AuditableEntity
{
    public int Id { get; set; }
    public Guid GuidId { get; set; } = Guid.CreateVersion7();
    public int PaymentRequestId { get; set; }
    public Guid PaymentRequestGuid { get; set; }
    public string IdemPotency { get; set; } = default!;
    public string? ExternalTransactionId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string? FailureResponse { get; set; }
    public string? ResponsePayload { get; set; }
}
