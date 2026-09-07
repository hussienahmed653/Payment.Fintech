namespace PaymentRequest.Application.Merchant.Command.CreateMerchant;

public class CreatePaymentCommandHandler(IUnitOfWork unitOfWork, IPaymentRepository paymentRepository) : IRequestHandler<CreatePaymentCommand, Result<PaymentResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;

    public async Task<Result<PaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.CreatePaymentRequestAsync(request.Request, cancellationToken);
        var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
        if (TotalChanges == 0)
            return Result.Failure<PaymentResponse>(PaymentRequestErrors.ZeroRowsAffected);
        if (TotalChanges > 1)
            return Result.Failure<PaymentResponse>(PaymentRequestErrors.MultibleRowsAffected);
        return Result.Success(payment.Adapt<PaymentResponse>());
    }
}
