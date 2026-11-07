using Shared;
using Shared.Events.Payments;
using Shared.Sections;

namespace PaymentRequest.Application.Common.Mapping;

internal class PaymentRequestConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
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

        config.NewConfig<(PaymentEnvelope request, PaymentResponse response), PaymentResponse>()
            .Map(dest => dest, src => src.response);

        config.NewConfig<PaymentResponse, PaymentRequestCreatedIntegrationEvent>()
            //.Map(dest => dest.EventId, src => Guid.NewGuid())
            .Map(dest => dest.PaymentRequestId, src => src.GuidId)
            .Map(dest => dest.CorrelationId, src => Guid.CreateVersion7())
            .Map(dest => dest.CustomerId, src => src.CustomerGuid)
            .Map(dest => dest.MerchantId, src => src.MerchantGuid)
            .Map(dest => dest.Amount, src => src.Amount)
            .Map(dest => dest.Currency, src => src.Currency.ToString());
            //.Map(dest => dest.OccurredOn, src => DateTime.UtcNow);

        //var paymentRequestCreatedEvent = request.Request.payment.Adapt<PaymentRequestCreatedIntegrationEvent>();
    }
}
