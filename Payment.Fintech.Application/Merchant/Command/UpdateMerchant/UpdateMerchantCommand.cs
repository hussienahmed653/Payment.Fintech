namespace Payment.Fintech.Application.Merchant.Command.UpdateMerchant;

public record UpdateMerchantCommand(Guid Guid, UpdateMerchantRequest Request) : IRequest<Result<MerchantResponse>>;
