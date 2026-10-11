using ResultPattern.Abstraction;
using Shared;
using Shared.Sections;

namespace PaymentProvider.Application.Deposit.Command;

public record InitiateDepositCommand(PaymentEnvelope request) : IRequest<Result<object>>;
