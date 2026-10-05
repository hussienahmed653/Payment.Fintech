using PaymentProvider.Application.Common.Interfaces.FactorySelector;
using ResultPattern.Abstraction;

namespace PaymentProvider.Application.Deposit.Command;

public class InitiateDepositCommandHandler(ISelectorFactory selectorFactory) : IRequestHandler<InitiateDepositCommand, Result<object>>
{
    private readonly ISelectorFactory _selectorFactory = selectorFactory;
    public async Task<Result<object>> Handle(InitiateDepositCommand request, CancellationToken cancellationToken)
    {
        var provider = _selectorFactory.Select((int)request.request.Payer.Type);
        return await provider.MakeRequest(request.request);

        //return result.IsSuccess
        //    ? Result.Success(result.Value)
        //    : Result.Failure<InitiateDepositResponse>(result.Error);
    }
}
