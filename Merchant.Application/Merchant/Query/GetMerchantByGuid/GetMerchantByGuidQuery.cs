
namespace Merchant.Application.Merchant.Query.GetMerchantByGuid;

public record GetMerchantByGuidQuery(Guid? Guid) : IRequest<Result<MerchantResponse>>;
