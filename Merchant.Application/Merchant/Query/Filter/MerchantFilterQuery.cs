namespace Merchant.Application.Merchant.Query.Filter;

public record MerchantFilterQuery(MerchantFilterRequest Request) : IRequest<Result<IEnumerable<MerchantResponse>>>;
