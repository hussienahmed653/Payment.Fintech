namespace Payment.Fintech.Application.Merchant.Query.GetMerchantByBusinessType;

public class GetMerchantByBusinessTypeQueryHandler(IMerchantRepository merchantRepository) : IRequestHandler<GetMerchantByBusinessTypeQuery, Result<IEnumerable<MerchantResponse>>>
{
    private readonly IMerchantRepository _merchantRepository = merchantRepository;

    public async Task<Result<IEnumerable<MerchantResponse>>> Handle(GetMerchantByBusinessTypeQuery request, CancellationToken cancellationToken)
    {
            var merchants = await _merchantRepository.GetMerchantByBusinessTypeAsync(request.BusinessType, cancellationToken);
            if(merchants.Count() == 0)
                return Result.Failure<IEnumerable<MerchantResponse>>(MerchantErrors.BusinessTypeNotFound);

            return Result.Success(merchants);
    }
}