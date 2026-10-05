using PaymentRequest.Application.Payments.Command.PayPayment;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PaymentRequest.Application.Merchant.Command.CreateMerchant;

public class CreatePaymentCommandHandler(IUnitOfWork unitOfWork,
                                         IPaymentRepository paymentRepository,
                                         IHttpClientFactory httpClientFactory,
                                         IMediator mediator) : IRequestHandler<CreatePaymentCommand, Result<PaymentResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMediator _mediator = mediator;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
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
            var payment = await _paymentRepository.CreatePaymentRequestAsync(request.Request, cancellationToken);
            var TotalChanges = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (TotalChanges == 0)
                return Result.Failure<PaymentResponse>(PaymentRequestErrors.ZeroRowsAffected);
            if (TotalChanges > 1)
                return Result.Failure<PaymentResponse>(PaymentRequestErrors.MultibleRowsAffected);

            var idempotencyKey = Guid.NewGuid().ToString();

            var PayResult = await _mediator.Send(new PayPaymentCommand(payment.Reference, idempotencyKey), cancellationToken);

   
            if (PayResult.IsFailure)
                return Result.Failure<PaymentResponse>(PayResult.Error);
            return Result.Success(payment.Adapt<PaymentResponse>());
        }
        catch
        {
            return Result.Failure<PaymentResponse>(PaymentRequestErrors.PaymentProcessingFailed);
        }
    }
}
