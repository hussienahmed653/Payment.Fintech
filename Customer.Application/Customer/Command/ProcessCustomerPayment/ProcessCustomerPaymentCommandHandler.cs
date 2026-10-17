using MassTransit;
using Shared;
using Shared.Messaging;
using Shared.Sections;
using System.Text.Json;

namespace Customer.Application.Customer.Command.ProcessCustomerPayment;

public class ProcessCustomerPaymentCommandHandler(IRequestClient<CreatePaymentMessage> createPaymentClient)
    : IRequestHandler<ProcessCustomerPaymentCommand, Result<object>>
{
    public async Task<Result<object>> Handle(ProcessCustomerPaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var envelope = MapEnvelope(request.Request);
            var response = await createPaymentClient.GetResponse<MessagingCompleted, MessagingFaulted>(
                new CreatePaymentMessage(envelope),
                cancellationToken,
                RequestTimeout.After(s: 30));

            if (response.Is(out Response<MessagingFaulted>? faulted))
            {
                return Result.Failure<object>(new Error(
                    faulted.Message.Error.Code,
                    faulted.Message.Error.Description,
                    faulted.Message.Error.StatusCode ?? StatusCodes.Status502BadGateway));
            }

            if (response.Is(out Response<MessagingCompleted>? completed)
                && !string.IsNullOrWhiteSpace(completed.Message.PayloadJson))
            {
                var payload = JsonSerializer.Deserialize<JsonElement>(completed.Message.PayloadJson);
                return Result.Success<object>(payload);
            }

            return Result.Success();
        }
        catch (RequestTimeoutException)
        {
            return Result.Failure<object>(new Error("Messaging.Timeout", "Payment request service did not respond in time.", StatusCodes.Status504GatewayTimeout));
        }
        catch (RequestFaultException ex)
        {
            return Result.Failure<object>(new Error("Messaging.Fault", ex.Message, StatusCodes.Status502BadGateway));
        }
        catch
        {
            return Result.Failure<object>(new Error("An error occurred while processing the customer payment", "An unexpected error occurred", 500));
        }
    }

    private static PaymentEnvelope MapEnvelope(CustomerPaymentRequest request)
    {
        var currency = Enum.TryParse<Currency>(request.Currency, true, out var parsedCurrency)
            ? parsedCurrency
            : Currency.ZMW;

        var payerType = Enum.TryParse<PayerType>(request.Type, true, out var parsedPayerType)
            ? parsedPayerType
            : PayerType.MMO;

        return new PaymentEnvelope
        {
            payment = new PaymentRequestSection(
                request.MerchantId,
                request.MerchantGuid,
                request.CustomerId,
                request.CustomerGuid,
                request.Amount,
                currency),
            provider = new ProviderSection(
                Guid.CreateVersion7(),
                new PayerDto(payerType, new AccountDetailsDto(request.Provider, request.PhoneNumber)),
                null,
                null,
                null),
            transaction = new TransactionSection(string.Empty, Guid.CreateVersion7().ToString())
        };
    }
}
