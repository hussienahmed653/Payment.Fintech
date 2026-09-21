using Azure.Core;

namespace PaymentRequest.Application.Payments.Command.PayPayment;

public class PayPaymentCommandHandler(IPaymentRepository paymentRepository,
                                                IUnitOfWork unitOfWork,
                                                IPaymentGatewayRepository paymentGatewayRepository,
                                                IPaymentTransactionRepository paymentTransactionRepository,
                                                ICacheService cacheService) : IRequestHandler<PayPaymentCommand, Result<PayPaymentResponse>>
{

    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IPaymentTransactionRepository _paymentTransactionRepository = paymentTransactionRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPaymentGatewayRepository _paymentGatewayRepository = paymentGatewayRepository;
    private readonly ICacheService _cacheService = cacheService;
    public async Task<Result<PayPaymentResponse>> Handle(PayPaymentCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"Reference:{request.reference}";
        var lockKey = $"Lock:{request.reference}";
        var lockvalue = Guid.CreateVersion7().ToString();
        if (await ReferenceInCache(request.reference, cacheKey, cancellationToken))
            return Result.Failure<PayPaymentResponse>(PaymentTransactionErrors.DuplicateSubmissionNotAllowed);

        if (!await TryAcquirePaymentLockAsync(request.reference, lockKey, lockvalue, cancellationToken))
            return Result.Failure<PayPaymentResponse>(PaymentTransactionErrors.ConcurrentPaymentProcessing);
        try
        {
            var (payment, error) = await IsValidPaymentRequest(request.reference, cancellationToken);
            if (error == Errors.PaymentNotFound)
                return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.PaymentRequestNotFound);
            if (error == Errors.StatusNotValidForPayment)
                return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.InvalidStatusForProcessing);


            if (await _paymentTransactionRepository.GetByPaymentRequestGuidAsync(payment.GuidId, cancellationToken) is { } existingTransaction)
            {
                await _cacheService.SetAsync(cacheKey, TimeSpan.FromMinutes(30), cancellationToken);
                return Result.Failure<PayPaymentResponse>(PaymentTransactionErrors.DuplicateSubmissionNotAllowed);
            }

            var paymentTransaction = await BeginProcessingTransactionAsync(payment, request.reference, cancellationToken);

            var gatewayResult = await _paymentGatewayRepository.ProcessPaymentAsync(payment.Reference, payment.Amount, payment.Currency.ToString(), cancellationToken);
            if (!gatewayResult.IsSuccess)
            {
                await HandlePaymentFailureAsync(payment, paymentTransaction, gatewayResult.ErrorMessage!, cancellationToken);
                return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.PaymentProcessingFailed);
            }
            var response = await HandlePaymentSuccessAsync(payment, paymentTransaction, gatewayResult, cacheKey, cancellationToken);
            return Result.Success(response);

        }
        finally
        {
            await _cacheService.ReleaseLockAsync(lockKey, lockvalue, cancellationToken);
        }
    }
    private async Task<bool> ReferenceInCache(string reference, string cacheKey, CancellationToken cancellationToken = default!)
    {
        var cachedResponse = await _cacheService.GetAsync<bool>(cacheKey, cancellationToken);
        return cachedResponse
            ? true
            : false;
    }
    private async Task<bool> TryAcquirePaymentLockAsync(string reference, string lockKey, string lockvalue, CancellationToken cancellationToken = default!)
    {
        var lockaquired = await _cacheService.AcquireLockAsync(lockKey, lockvalue, TimeSpan.FromSeconds(30), cancellationToken);
        return lockaquired
            ? true
            : false;
    }
    private async Task<(Domain.Entities.PaymentRequest? payment, Errors error)> IsValidPaymentRequest(string reference, CancellationToken cancellationToken = default!)
    {
        var payment = await _paymentRepository.GetPaymentByReferenceAsync(reference, cancellationToken);
        if (payment is null)
            return (null, Errors.PaymentNotFound)!;

        if (!isValidStatus(payment.Status))
            return (null, Errors.StatusNotValidForPayment)!;

        return (payment, Errors.None);
    }
    private async Task<PaymentTransaction> BeginProcessingTransactionAsync(Domain.Entities.PaymentRequest payment, string reference, CancellationToken cancellationToken = default!)
    {
        payment.Status = PaymentStatus.Processing;
        var paymentTransaction = (payment, reference).Adapt<PaymentTransaction>();

        await _paymentTransactionRepository.AddAsync(paymentTransaction, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return paymentTransaction;
    }
    private async Task HandlePaymentFailureAsync(Domain.Entities.PaymentRequest payment, PaymentTransaction paymentTransaction, string gatewayErrorMessage, CancellationToken cancellationToken = default!)
    {
        payment.Status = PaymentStatus.Failed;
        paymentTransaction.Status = PaymentStatus.Failed;
        paymentTransaction.FailureResponse = gatewayErrorMessage;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    private async Task<PayPaymentResponse> HandlePaymentSuccessAsync(Domain.Entities.PaymentRequest payment,
                                                 PaymentTransaction paymentTransaction,
                                                 PaymentGatewayResponse gatewayResult,
                                                 string cacheKey,
                                                 CancellationToken cancellationToken = default!)
    {
        payment.Status = PaymentStatus.Successful;
        paymentTransaction.Status = PaymentStatus.Successful;
        paymentTransaction.ExternalTransactionId = gatewayResult.TransactionId;
        var response = (payment, gatewayResult).Adapt<PayPaymentResponse>();
        paymentTransaction.ResponsePayload = JsonSerializer.Serialize(response);
        await _cacheService.SetAsync(cacheKey, TimeSpan.FromMinutes(30), cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return response;
    }
    private bool isValidStatus(PaymentStatus status) =>
        status == PaymentStatus.Created;
    private enum Errors
    {
        None = 0,
        PaymentNotFound = 1,
        StatusNotValidForPayment = 2,
    }
}
