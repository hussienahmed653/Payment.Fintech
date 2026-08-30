namespace Payment.Fintech.Application.Merchant.Command.UpdateMerchant;

public record UpdateMerchantCommand(UpdateMerchantRequest Request) : IRequest<Result<MerchantResponse>>;
