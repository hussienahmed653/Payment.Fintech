using MediatR;

namespace PaymentRequest.Application.Merchant.Command.CreateMerchant;

public record CreatePaymentCommand(Common.Contracts.Merchants.PaymentRequest Request) : IRequest<Result<PaymentResponse>>;
