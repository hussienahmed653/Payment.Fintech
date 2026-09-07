namespace PaymentRequest.Application.Common.Contracts.PaymentRequests;

public record PaymentGatewayResponse(
    bool IsSuccess,
    string TransactionId,
    string? ErrorMessage = null
);
