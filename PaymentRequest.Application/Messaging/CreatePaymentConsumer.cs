using MassTransit;
using PaymentRequest.Application.Payments.Command.CreatePayment;
using Shared.Messaging;

namespace PaymentRequest.Application.Messaging;

public class CreatePaymentConsumer(IMediator mediator) : IConsumer<CreatePaymentMessage>
{
    public async Task Consume(ConsumeContext<CreatePaymentMessage> context)
    {
        var result = await mediator.Send(new CreatePaymentCommand(context.Message.Envelope), context.CancellationToken);

        if (result.IsFailure)
        {
            await context.RespondAsync(new MessagingFaulted(
                result.Error));
            return;
        }

        await context.RespondAsync(new MessagingCompleted(JsonSerializer.Serialize(result.Value)));
    }
}
