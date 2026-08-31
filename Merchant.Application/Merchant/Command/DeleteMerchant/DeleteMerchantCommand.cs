namespace Merchant.Application.Merchant.Command.DeleteMerchant;

public record DeleteMerchantCommand(Guid Guid) : IRequest<Result>;
