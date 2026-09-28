using PaymentProvider.Application.Common.Contracts.Deposit;

namespace PaymentProvider.Application.Common.Interfaces.Processor;

public interface IPaymentProcessor
{
    Task<object> MakeRequest(InitiateDepositRequest request);
}
