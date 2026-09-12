

namespace PaymentRequest.Application.Payments.Command.PayPayment;

public class PayPaymentCommandHandler(IPaymentRepository paymentRepository,
                                                IUnitOfWork unitOfWork,
                                                IPaymentGatewayRepository paymentGatewayRepository,
                                                IPaymentTransactionRepository paymentTransactionRepository) : IRequestHandler<PayPaymentCommand, Result<PayPaymentResponse>>
{
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IPaymentTransactionRepository _paymentTransactionRepository = paymentTransactionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPaymentGatewayRepository _paymentGatewayRepository = paymentGatewayRepository;
    public async Task<Result<PayPaymentResponse>> Handle(PayPaymentCommand request, CancellationToken cancellationToken)
    {
        if (await _paymentRepository.GetPaymentByReferenceAsync(request.reference, cancellationToken) is not { } payment)
            return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.PaymentRequestNotFound);

        if (await _paymentTransactionRepository.GetByIdemPotencyIdAsync(request.idempotencyId) is { } existingTransaction)
        {
            if (existingTransaction.PaymentRequestGuid != payment.GuidId)
                return Result.Failure<PayPaymentResponse>(PaymentTransactionErrors.IdempotencyKeyMismatch);
            if (existingTransaction.Status == PaymentStatus.Processing)
                return Result.Failure<PayPaymentResponse>(PaymentTransactionErrors.ConcurrentPaymentProcessing);
            if (existingTransaction.Status == PaymentStatus.Failed)
                return Result.Failure<PayPaymentResponse>(existingTransaction.FailureResponse!.PaymentFailed());
            if (existingTransaction.Status == PaymentStatus.Successful)
            {
                var cachedResponse = JsonSerializer.Deserialize<PayPaymentResponse>(existingTransaction.ResponsePayload!);
                return Result.Success(cachedResponse!);
            }
        }

        if (!IsValidStatus(payment.Status))
            return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.InvalidStatusForProcessing);

        payment.Status = PaymentStatus.Processing;
        var paymentTransaction = new PaymentTransaction
        {
            PaymentRequestId = payment.Id,
            PaymentRequestGuid = payment.GuidId,
            IdemPotency = request.idempotencyId,
            Status = PaymentStatus.Processing,
            Amount = payment.Amount,
        };

        await _paymentTransactionRepository.AddAsync(paymentTransaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var gatewayResult = await _paymentGatewayRepository.ProcessPaymentAsync(payment.Reference, payment.Amount, payment.Currency.ToString(), cancellationToken);
        if (gatewayResult.IsSuccess)
        {
            payment.Status = PaymentStatus.Successful;
            paymentTransaction.Status = PaymentStatus.Successful;
            paymentTransaction.ExternalTransactionId = gatewayResult.TransactionId;
            var response = (payment, gatewayResult).Adapt<PayPaymentResponse>();
            paymentTransaction.ResponsePayload = JsonSerializer.Serialize(response);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success(response);
        }
        payment.Status = PaymentStatus.Failed;
        paymentTransaction.Status = PaymentStatus.Failed;
        paymentTransaction.FailureResponse = gatewayResult.ErrorMessage;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.PaymentProcessingFailed);
    }
    private bool IsValidStatus(PaymentStatus status) => 
        status == PaymentStatus.Created;
}
