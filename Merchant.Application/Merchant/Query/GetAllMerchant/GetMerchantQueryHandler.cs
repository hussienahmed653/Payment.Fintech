namespace Merchant.Application.Merchant.Query.GetAllMerchant;

public class GetMerchantQueryHandler(IMerchantRepository merchantRepository) : IRequestHandler<GetMerchantQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(GetMerchantQuery request, CancellationToken cancellationToken)
    {
        var merchants = await _merchantRepository.GetMerchantAsync(cancellationToken);
        if (merchants.Count() == 0)
            return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.MerchantNotFound);
        return Result.Success(merchants);
    }
}
