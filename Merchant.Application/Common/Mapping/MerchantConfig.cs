namespace Merchant.Application.Common.Mapping;

internal class MerchantConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<MerchantRequest, Domain.Entities.Merchant>()
            .Map(dest => dest.FullName, src => $"{src.ContactFirstName} {src.ContactLastName}");
        //config.NewConfig<MerchantResponse, Domain.Merchant>()
        //    .Map(dest => dest.BusinessType, src => src.BusinessType.ToString())
        //    .Map(dest => dest.Status, src => src.Status.ToString());
    }
}
