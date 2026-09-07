namespace PaymentRequest.Application.Common.Contracts.PaymentRequests;

public record PayPaymentResponse(
    string Reference,
    string Status,
    string TransactionId,
    decimal Amount,
    string Currency
);
