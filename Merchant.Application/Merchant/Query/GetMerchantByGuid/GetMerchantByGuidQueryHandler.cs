namespace Merchant.Application.Merchant.Query.GetMerchantByGuid;

public class GetMerchantByGuidQueryHandler(IMerchantRepository merchantRepository) : IRequestHandler<GetMerchantByGuidQuery, Result<MerchantResponse>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;
    public async Task<Result<MerchantResponse>> Handle(GetMerchantByGuidQuery request, CancellationToken cancellationToken)
    {
        if (await _merchantRepository.GetMerchantByGuidAsync(request.Guid.Value, cancellationToken) is not { } merchant)
            return Result.Failure<MerchantResponse>(MerchantErrors.MerchantNotFound);

        return Result.Success(merchant.Adapt<MerchantResponse>());
    }
}
