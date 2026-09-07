namespace PaymentRequest.Application.Payments.Query.GetPaymentRequestByReference;

public class GetPaymentRequestByReferenceQueryHandler(IPaymentRepository paymentRepository) : IRequestHandler<GetPaymentRequestByReferenceQuery, Result<PaymentResponse>>
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;

    public async Task<Result<PaymentResponse>> Handle(GetPaymentRequestByReferenceQuery request, CancellationToken cancellationToken)
    {
        if (await _paymentRepository.GetPaymentByReferenceAsync(request.reference, cancellationToken) is not { } payment)
            return Result.Failure<PaymentResponse>(PaymentRequestErrors.PaymentRequestNotFound);
        return Result.Success(payment.Adapt<PaymentResponse>());

    }
}
