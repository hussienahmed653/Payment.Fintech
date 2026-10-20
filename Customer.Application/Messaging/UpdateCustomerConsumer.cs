using Customer.Application.Customer.Command.UpdateCustomer;
using MassTransit;
using Shared.Messaging;

namespace Customer.Application.Messaging;

public class UpdateCustomerConsumer(IMediator mediator, IPublishEndpoint publishEndpoint) : IConsumer<UpdateCustomerMessage>
{
    public async Task Consume(ConsumeContext<UpdateCustomerMessage> context)
    {
        var result = await mediator.Send(new UpdateCustomerCommand(context.Message.Envelope), context.CancellationToken);

        if (result.IsFailure)
        {
            await publishEndpoint.Publish(new MessagingFaulted(
                result.Error), context.CancellationToken);
            //await context.RespondAsync(new MessagingFaulted(
            //    result.Error));
            //return;
        }

        await publishEndpoint.Publish(new MessagingCompleted(), context.CancellationToken);
        //await context.RespondAsync(new MessagingCompleted());
    }
}
