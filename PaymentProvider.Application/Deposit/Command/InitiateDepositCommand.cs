using PaymentProvider.Application.Common.Contracts.Deposit;
using ResultPattern.Abstraction;

namespace PaymentProvider.Application.Deposit.Command;

public record InitiateDepositCommand(InitiateDepositRequest request) : IRequest<Result<object>>;
