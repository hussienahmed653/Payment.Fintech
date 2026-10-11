namespace PaymentRequest.Application.Common.Contracts.PaymentRequests;

public class PayPaymentResponse
{
    public Guid GuidId { get; set; }
    public string Reference { get; set; }
    public string Status { get; set; }
    public string TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
}

