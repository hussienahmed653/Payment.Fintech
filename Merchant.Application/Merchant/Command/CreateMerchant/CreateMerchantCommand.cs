using MediatR;

namespace Merchant.Application.Merchant.Command.CreateMerchant;

public record CreateMerchantCommand(MerchantRequest Request) : IRequest<Result<MerchantResponse>>;
