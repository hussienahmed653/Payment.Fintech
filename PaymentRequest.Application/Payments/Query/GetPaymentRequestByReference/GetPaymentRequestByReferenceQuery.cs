namespace PaymentRequest.Application.Payments.Query.GetPaymentRequestByReference;

public record GetPaymentRequestByReferenceQuery(string reference) : IRequest<Result<PaymentResponse>>;