using PaymentRequest.Application.Common.Interfaces.Payments;

namespace PaymentRequest.Application.Payments.Command.PayPayment;

public class PayPaymentCommandHandler(IPaymentRepository paymentRepository,
                                                IUnitOfWork unitOfWork,
                                                IPaymentGatewayRepository paymentGatewayRepository) : IRequestHandler<PayPaymentCommand, Result<PayPaymentResponse>>
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPaymentGatewayRepository _paymentGatewayRepository = paymentGatewayRepository;
    public async Task<Result<PayPaymentResponse>> Handle(PayPaymentCommand request, CancellationToken cancellationToken)
    {
        if(await _paymentRepository.GetPaymentByReferenceAsync(request.reference, cancellationToken) is not { } payment)
            return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.PaymentRequestNotFound);

        if(!IsValidStatus(payment.Status))
            return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.InvalidStatusForProcessing);

        payment.Status = PaymentStatus.Processing;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var gatewayResult = await _paymentGatewayRepository.ProcessPaymentAsync(payment.Reference, payment.Amount, payment.Currency.ToString());
        if(gatewayResult.IsSuccess)
        {
            payment.Status = PaymentStatus.Successful;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success((payment, gatewayResult).Adapt<PayPaymentResponse>());
        }
        payment.Status = PaymentStatus.Failed;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.PaymentProcessingFailed);
    }
    private bool IsValidStatus(PaymentStatus status) => 
        status == PaymentStatus.Created;
}
