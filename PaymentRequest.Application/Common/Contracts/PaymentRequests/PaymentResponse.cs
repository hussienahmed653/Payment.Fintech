using PaymentRequest.Domain.Enums;

namespace PaymentRequest.Application.Common.Contracts.PaymentRequests;

public class PaymentResponse
{
    public Guid GuidId { get; set; }
    public string Reference { get; set; }
    public int MerchantId { get; set; }
    public Guid MerchantGuid { get; set; }
    public int? CustomerId { get; set; }
    public Guid? CustomerGuid { get; set; }
    public decimal Amount { get; set; }
    public Currency Currency { get; set; }
    public string Status { get; set; }
}