using MassTransit;
using PaymentRequest.Application.Payments.Command.PayPayment;
using Shared.Messaging;

namespace PaymentRequest.Application.Payments.Command.CreatePayment;

public class CreatePaymentCommandHandler(IUnitOfWork unitOfWork,
                                         IPaymentRepository paymentRepository,
                                         IMediator mediator,
                                         IPaymentTransactionRepository paymentTransactionRepository,
                                         IRequestClient<UpdateCustomerMessage> updateCustomerClient,
                                         IRequestClient<InitiateDepositMessage> initiateDepositClient) : IRequestHandler<CreatePaymentCommand, Result<PaymentResponse>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMediator _mediator = mediator;
    private readonly IPaymentRepository _paymentRepository = paymentRepository;
    private readonly IPaymentTransactionRepository _paymentTransactionRepository = paymentTransactionRepository;

    public async Task<Result<PaymentResponse>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentRepository.CreatePaymentRequestAsync(request.Request.payment, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var customerResult = await SendAsync(
                updateCustomerClient,
                new UpdateCustomerMessage(request.Request),
                cancellationToken);

            if (customerResult.IsFailure)
            {
                await _paymentRepository.UpdateStatus(payment.GuidId, PaymentStatus.Failed);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Failure<PaymentResponse>(customerResult.Error);
            }

            var paymentTransactionResponse = await _mediator.Send(new PayPaymentCommand(payment.Reference, request.Request.transaction!.IdempotencyId), cancellationToken);

            if (paymentTransactionResponse.IsFailure)
            {
                await _paymentRepository.UpdateStatus(payment.GuidId, PaymentStatus.Failed);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Failure<PaymentResponse>(paymentTransactionResponse.Error);
            }

            var paymentProviderResult = await SendAsync(
                initiateDepositClient,
                new InitiateDepositMessage(request.Request),
                cancellationToken);

            if (paymentProviderResult.IsFailure)
            {
                await _paymentRepository.UpdateStatus(payment.GuidId, PaymentStatus.Failed);
                await _paymentTransactionRepository.UpdateAsync(paymentTransactionResponse.Value.GuidId, PaymentStatus.Failed, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Failure<PaymentResponse>(paymentProviderResult.Error);
            }

            var response = (request.Request, payment).Adapt<PaymentResponse>();
            return Result.Success(response);
        }
        catch
        {
            return Result.Failure<PaymentResponse>(PaymentRequestErrors.PaymentProcessingFailed);
        }
    }

    private static async Task<Result> SendAsync<TMessage>(
        IRequestClient<TMessage> client,
        TMessage message,
        CancellationToken cancellationToken)
        where TMessage : class
    {
        try
        {
            var response = await client.GetResponse<MessagingCompleted, MessagingFaulted>(
                message,
                cancellationToken,
                RequestTimeout.After(s: 30));

            if (response.Is(out Response<MessagingFaulted>? faulted))
            {
                return Result.Failure(new Error(
                    faulted.Message.Error.Code,
                    faulted.Message.Error.Description,
                    faulted.Message.Error.StatusCode ?? StatusCodes.Status502BadGateway));
            }

            return Result.Success();
        }
        catch (RequestTimeoutException)
        {
            return Result.Failure(new Error("Messaging.Timeout", "Downstream service did not respond in time.", StatusCodes.Status504GatewayTimeout));
        }
        catch (RequestFaultException ex)
        {
            return Result.Failure(new Error("Messaging.Fault", ex.Message, StatusCodes.Status502BadGateway));
        }
    }
}
