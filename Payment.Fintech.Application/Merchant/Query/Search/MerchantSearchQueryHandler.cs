namespace Payment.Fintech.Application.Merchant.Query.Search;

public class MerchantSearchQueryHandler(IMerchantRepository merchantRepository) : IRequestHandler<MerchantSearchQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(MerchantSearchQuery request, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(request.Search))
            return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.SearchKeyWordNotFound);

        var merchants = await _merchantRepository.SearchAsync(request.Search);
        if (!merchants.Any())
            return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.SearchNotFound);

        return Result.Success(merchants.Adapt<IEnumerable<MerchantResponse>>());
    }
}
