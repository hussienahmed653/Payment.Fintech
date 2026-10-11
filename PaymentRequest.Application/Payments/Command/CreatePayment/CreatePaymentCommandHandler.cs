using PaymentRequest.Application.Payments.Command.PayPayment;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PaymentRequest.Application.Payments.Command.CreatePayment;

public class CreatePaymentCommandHandler(IUnitOfWork unitOfWork,
                                         IPaymentRepository paymentRepository,
                                         IHttpClientFactory httpClientFactory,
                                         IMediator mediator,
                                         IPaymentTransactionRepository paymentTransactionRepository) : IRequestHandler<CreatePaymentCommand, Result<PaymentResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMediator _mediator = mediator;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IPaymentTransactionRepository _paymentTransactionRepository = paymentTransactionRepository;
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() }

    };

    public async Task<Result<PaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentRepository.CreatePaymentRequestAsync(request.Request.payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var customerClient = httpClientFactory.CreateClient("CustomerService");
            //change Request to pass CustomerSection
            var customerResponse = await customerClient.PutAsJsonAsync($"https://localhost:7060/api/customers/update-customer", request, _options, cancellationToken);

            if(!customerResponse.IsSuccessStatusCode)
            {
                var body = await customerResponse.Content.ReadAsStringAsync(cancellationToken);
                await _paymentRepository.UpdateStatus(payment.GuidId, PaymentStatus.Failed);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Failure<PaymentResponse>(new Error("Failed to update customer", body, (int)customerResponse.StatusCode));
            }

            //var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            //if (TotalChanges == 0)
            //    return Result.Failure<PaymentResponse>(PaymentRequestErrors.ZeroRowsAffected);
            //if (TotalChanges > 1)
            //    return Result.Failure<PaymentResponse>(PaymentRequestErrors.MultibleRowsAffected);

            var PaymentTransactionResponse = await _mediator.Send(new PayPaymentCommand(payment.Reference, request.Request.transaction!.IdempotencyId), cancellationToken);

   
            if (PaymentTransactionResponse.IsFailure)
            {
                await _paymentRepository.UpdateStatus(payment.GuidId, PaymentStatus.Failed);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Failure<PaymentResponse>(PaymentTransactionResponse.Error);
            }

            var paymentProviderClient = httpClientFactory.CreateClient("PaymentProvider");

            var paymentProviderResponse = await paymentProviderClient.PostAsJsonAsync($"https://localhost:7062/api/deposits", request, _options, cancellationToken);

            if(!paymentProviderResponse.IsSuccessStatusCode)
            {
                var body = await paymentProviderResponse.Content.ReadAsStringAsync(cancellationToken);
                await _paymentRepository.UpdateStatus(payment.GuidId, PaymentStatus.Failed);
                await _paymentTransactionRepository.UpdateAsync(PaymentTransactionResponse.Value.GuidId, PaymentStatus.Failed, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Failure<PaymentResponse>(new Error("Failed to process payment with provider", body, (int)paymentProviderResponse.StatusCode));
            }


            var response = (request.Request, payment).Adapt<PaymentResponse>();
            return Result.Success(payment);
        }
        catch
        {
            return Result.Failure<PaymentResponse>(PaymentRequestErrors.PaymentProcessingFailed);
        }
    }
}
