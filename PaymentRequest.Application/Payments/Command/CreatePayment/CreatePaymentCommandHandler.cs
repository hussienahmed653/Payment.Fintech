using System.Net.Http.Json;

namespace PaymentRequest.Application.Merchant.Command.CreateMerchant;

public class CreatePaymentCommandHandler(IUnitOfWork unitOfWork,
                                         IPaymentRepository paymentRepository,
                                         IHttpClientFactory httpClientFactory) : IRequestHandler<CreatePaymentCommand, Result<PaymentResponse>>
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

        var idempotencyKey = Guid.NewGuid().ToString();
        var httpClient = httpClientFactory.CreateClient("PaymentGateway");
        var requestUri = $"api/payments/{payment.Reference}/pay";
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = JsonContent.Create(payment) // وضع البيانات في الـ Body
        };

        // 3. إضافة هيدر الـ Idempotency
        httpRequest.Headers.Add("X-Idempotency-Key", idempotencyKey);
        var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Result.Failure<PaymentResponse>(new Error("Failed to process payment.", responseContent, (int)response.StatusCode));
        }
        return Result.Success(payment.Adapt<PaymentResponse>());
    }
}
