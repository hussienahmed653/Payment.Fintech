namespace Merchant.Application.Merchant.Query.GetMerchantByBusinessType;

public record GetMerchantByBusinessTypeQuery(string BusinessType) : IRequest<Result<IEnumerable<MerchantResponse>>>;
