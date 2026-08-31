namespace Customer.Application.Common.Mapping;

internal class CustomerConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CustomerRequest, Domain.Entities.Customer>()
            .Map(dest => dest.FullName, src => $"{src.ContactFirstName} {src.ContactLastName}");
        //config.NewConfig<MerchantResponse, Domain.Merchant>()
        //    .Map(dest => dest.BusinessType, src => src.BusinessType.ToString())
        //    .Map(dest => dest.Status, src => src.Status.ToString());
    }
}
