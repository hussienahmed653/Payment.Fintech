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
        config.NewConfig<(Domain.Entities.PaymentRequest payment, string idempotency), PaymentTransaction>()
            //.Map(dest => dest, src => src.payment)
            .Map(dest => dest.Status, src => PaymentStatus.Processing)
            .Map(dest => dest.PaymentRequestId, src => src.payment.Id)
            .Map(dest => dest.PaymentRequestGuid, src => src.payment.GuidId)
            .Map(dest => dest.Amount, src => src.payment.Amount)
            .Map(dest => dest.IdemPotency, src => src.idempotency);
    }
}
