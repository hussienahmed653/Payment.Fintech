using Customer.Application.Customer.Command.UpdateCustomer;
using MassTransit;
using Shared.Messaging;

namespace Customer.Application.Messaging;

public class UpdateCustomerConsumer(IMediator mediator) : IConsumer<UpdateCustomerMessage>
{
    public async Task Consume(ConsumeContext<UpdateCustomerMessage> context)
    {
        var result = await mediator.Send(new UpdateCustomerCommand(context.Message.Envelope), context.CancellationToken);

        if (result.IsFailure)
        {
            await context.RespondAsync(new MessagingFaulted(
                result.Error));
            return;
        }

        await context.RespondAsync(new MessagingCompleted());
    }
}
