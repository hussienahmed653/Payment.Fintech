using MediatR;
namespace Payment.Fintech.Application.Merchant.Query.GetAllMerchant;

public record GetMerchantQuery() : IRequest<Result<IEnumerable<MerchantResponse>>>;
