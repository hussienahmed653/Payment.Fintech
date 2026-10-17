using MassTransit;
using PaymentProvider.Application.Deposit.Command;
using Shared.Messaging;
using System.Text.Json;

namespace PaymentProvider.Application.Messaging;

public class InitiateDepositConsumer(IMediator mediator) : IConsumer<InitiateDepositMessage>
{
    public async Task Consume(ConsumeContext<InitiateDepositMessage> context)
    {
        var result = await mediator.Send(new InitiateDepositCommand(context.Message.Envelope), context.CancellationToken);

        if (result.IsFailure)
        {
            await context.RespondAsync(new MessagingFaulted(
                result.Error));
            return;
        }

        await context.RespondAsync(new MessagingCompleted(JsonSerializer.Serialize(result.Value)));
    }
}
