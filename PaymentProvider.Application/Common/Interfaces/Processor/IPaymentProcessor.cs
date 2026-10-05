using PaymentProvider.Application.Common.Contracts.Deposit;
using ResultPattern.Abstraction;

namespace PaymentProvider.Application.Common.Interfaces.Processor;

public interface IPaymentProcessor
{
    Task<Result<object>> MakeRequest(InitiateDepositRequest request);
}
