namespace Payment.Fintech.Application.Merchant.Query.Filter;

public class MerchantFilterQueryHandler(IMerchantRepository merchantRepository) : IRequestHandler<MerchantFilterQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(MerchantFilterQuery request, CancellationToken cancellationToken)
    {
        var filter = new MerchantFilterSpecification(request.Request);
        var merchantsResult = await _merchantRepository.FilterAsync(filter, cancellationToken);

        if (!merchantsResult.Any())
            return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.Filter);

        return Result.Success(merchantsResult.Adapt<IEnumerable<MerchantResponse>>());
    }
}