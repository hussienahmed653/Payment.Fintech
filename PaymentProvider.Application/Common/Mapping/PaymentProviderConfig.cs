using PaymentProvider.Application.Common.Contracts.Deposit;
using Shared;

namespace PaymentProvider.Application.Common.Mapping;

internal class PaymentProviderConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaymentEnvelope, InitiateDepositRequest>()
            .Map(dest => dest, src => src.provider)
            .Map(dest => dest.Amount, src => src.payment.Amount)
            .Map(dest => dest.Currency, src => src.payment.Currency);
            //.Map(dest => dest.PaymentMethod, src => src.PaymentMethod)
            //.Map(dest => dest.Metadata, src => src.Metadata);
    }
}
