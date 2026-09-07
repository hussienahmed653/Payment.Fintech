namespace PaymentRequest.Application.Common.Mapping;

internal class PaymentRequestConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Entities.PaymentRequest, PaymentResponse>()
            .Map(dest => dest.CreatedAt, src => src.CreatedOn);
        config.NewConfig<(Domain.Entities.PaymentRequest payment, PaymentGatewayResponse gateway), PayPaymentResponse>()
            .Map(dest => dest, src => src.payment)
            .Map(dest => dest.TransactionId, src => src.gateway.TransactionId);
    }
}
