using ResultPattern.Abstraction;
using Shared;
using Shared.Events.Payments;
using Shared.Messaging;
using Shared.Sections;

namespace PaymentProvider.Application.Deposit.Command;

public record InitiateDepositCommand : IRequest<Result<object>>
{ 
    public PaymentEnvelope request { get; init; }
    public static implicit operator InitiateDepositCommand(InitiateDepositMessage @event)
    {
        return new InitiateDepositCommand
        {
            request = @event.Envelope
        };
    }
}
