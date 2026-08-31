using MediatR;

namespace Merchant.Application.Merchant.Query.GetAllMerchant;

public record GetMerchantQuery() : IRequest<Result<IEnumerable<MerchantResponse>>>;
