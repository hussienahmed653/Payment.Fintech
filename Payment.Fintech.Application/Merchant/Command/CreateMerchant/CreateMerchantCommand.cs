using MediatR;
namespace Payment.Fintech.Application.Merchant.Command.CreateMerchant;

public record CreateMerchantCommand(MerchantRequest Request) : IRequest<Result<MerchantResponse>>;
