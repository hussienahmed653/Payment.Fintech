using PaymentProvider.Application.Common.Contracts.Deposit;
using PaymentProvider.Application.Common.Interfaces.FactorySelector;
using ResultPattern.Abstraction;

namespace PaymentProvider.Application.Deposit.Command;

public class InitiateDepositCommandHandler(ISelectorFactory selectorFactory) : IRequestHandler<InitiateDepositCommand, Result<object>>
{
    private readonly ISelectorFactory _selectorFactory = selectorFactory;
    public async Task<Result<object>> Handle(InitiateDepositCommand request, CancellationToken cancellationToken)
    {
        var initiateDepositRequest = request.request.Adapt<InitiateDepositRequest>()!;
        var provider = _selectorFactory.Select((int)initiateDepositRequest.Payer.Type);
        return await provider.MakeRequest(initiateDepositRequest);

        //return result.IsSuccess
        //    ? Result.Success(result.Value)
        //    : Result.Failure<InitiateDepositResponse>(result.Error);
    }
}
