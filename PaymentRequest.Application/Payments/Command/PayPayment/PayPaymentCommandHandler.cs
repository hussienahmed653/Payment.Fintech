using System.Net.Http.Json;
using System.Net.NetworkInformation;

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

            var (paymentTransaction, TotalChanges) = await BeginProcessingTransactionAsync(payment, request.reference, cancellationToken);

            //var httpClient = httpClientFactory.CreateClient("PaymentGateway");
            //var req = new Dictionary<string, object>
            //{
            //    { "request", new {
            //            DepositId = Guid.NewGuid().ToString(),
            //            Amount = payment.Amount.ToString(),
            //            Currency = "ZMW",
            //            Payer = new
            //            {
            //                Type = "MMO",
            //                AccountDetails = new
            //                {
            //                    Provider = "MTN_MOMO_ZMB",
            //                    PhoneNumber = "260763456789"
            //                }
            //            }
            //        }
            //    }              
            //};

            //var response = await httpClient.PostAsJsonAsync($"https://localhost:7062/api/deposits", req, cancellationToken);
            //var body = await response.Content.ReadAsStringAsync();
            if (TotalChanges == 0)
            {
                //var errorResponse = await HttpErrorReader.ReadErrorAsync(response, cancellationToken);

                await HandlePaymentFailureAsync(payment, paymentTransaction, "Failed to save payment transaction", cancellationToken);

                return Result.Failure<PayPaymentResponse>(new Error("Failed to save payment transaction", "No rows were affected in the database.", StatusCodes.Status500InternalServerError));
            }

            //return Result.Success(new PayPaymentResponse("","","",0.0m,""));
            //var gatewayResult = await _paymentGatewayRepository.ProcessPaymentAsync(payment.Reference, payment.Amount, payment.Currency.ToString(), cancellationToken);
            //if (!gatewayResult.IsSuccess)
            //{
            //    await HandlePaymentFailureAsync(payment, paymentTransaction, gatewayResult.ErrorMessage!, cancellationToken);
            //    return Result.Failure<PayPaymentResponse>(PaymentRequestErrors.PaymentProcessingFailed);
            //}
            return Result.Success(await HandlePaymentSuccessAsync(payment, paymentTransaction, cacheKey, cancellationToken));

        }
        finally
        {
            await _cacheService.ReleaseLockAsync(lockKey, lockvalue, cancellationToken);
        }
    }
    private async Task<bool> ReferenceInCache(string reference, string cacheKey, CancellationToken cancellationToken = default!) =>
        await _cacheService.GetAsync<bool>(cacheKey, cancellationToken);
    
    private async Task<bool> TryAcquirePaymentLockAsync(string reference, string lockKey, string lockvalue, CancellationToken cancellationToken = default!) =>
        await _cacheService.AcquireLockAsync(lockKey, lockvalue, TimeSpan.FromSeconds(30), cancellationToken);

    private async Task<(Domain.Entities.PaymentRequest? payment, Errors error)> IsValidPaymentRequest(string reference, CancellationToken cancellationToken = default!)
    {
        var payment = await _paymentRepository.GetPaymentByReferenceAsync(reference, cancellationToken);
        if (payment is null)
            return (null, Errors.PaymentNotFound)!;

        if (!isValidStatus(payment.Status))
            return (null, Errors.StatusNotValidForPayment)!;

        return (payment, Errors.None);
    }
    private async Task<(PaymentTransaction paymentTransaction, int TotalChanges)> BeginProcessingTransactionAsync(Domain.Entities.PaymentRequest payment, string reference, CancellationToken cancellationToken = default!)
    {
        payment.Status = PaymentStatus.Processing;
        var paymentTransaction = (payment, reference).Adapt<PaymentTransaction>();

        await _paymentTransactionRepository.AddAsync(paymentTransaction, cancellationToken);
        var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
        return (paymentTransaction, TotalChanges);
    }
    private async Task HandlePaymentFailureAsync(Domain.Entities.PaymentRequest payment, PaymentTransaction paymentTransaction, string gatewayErrorMessage, CancellationToken cancellationToken = default!)
    {
        //payment.Status = PaymentStatus.Failed;
        paymentTransaction.Status = PaymentStatus.Failed;
        paymentTransaction.FailureResponse = gatewayErrorMessage;
        //await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
    private async Task<PayPaymentResponse> HandlePaymentSuccessAsync(Domain.Entities.PaymentRequest payment,
                                                 PaymentTransaction paymentTransaction,
                                                 //PaymentGatewayResponse gatewayResult,
                                                 string cacheKey,
                                                 CancellationToken cancellationToken = default!)
    {
        payment.Status = PaymentStatus.Successful;
        paymentTransaction.Status = PaymentStatus.Successful;
        paymentTransaction.ExternalTransactionId = null;
        var response = payment.Adapt<PayPaymentResponse>();
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

//public static class HttpErrorReader
//{
//    public static async Task<Error> ReadErrorAsync(
//        HttpResponseMessage response, CancellationToken ct)
//    {
//        var raw = await response.Content.ReadAsStringAsync(ct);
//        try
//        {
//            using var doc = JsonDocument.Parse(raw);
//            if (doc.RootElement.TryGetProperty("errors", out var errors) &&
//                errors.ValueKind == JsonValueKind.Object)
//            {
//                var first = errors.EnumerateObject().First();
//                var description = first.Value.EnumerateArray().First().GetString() ?? raw;
//                return new Error(first.Name, description, (int)response.StatusCode);
//            }
//        }
//        catch (Exception ex) when (ex is JsonException or InvalidOperationException) { }

//        return new Error("Downstream.Failed", raw, (int)response.StatusCode);
//    }
//}
