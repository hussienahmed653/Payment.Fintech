namespace Payment.Fintech.Application.Merchant.Query.Search;

public record MerchantSearchQuery(string Search) : IRequest<Result<IEnumerable<MerchantResponse>>>;
