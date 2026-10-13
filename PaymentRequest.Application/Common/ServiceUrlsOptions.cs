namespace PaymentRequest.Application.Common;

public class ServiceUrlsOptions
{
    public const string SectionName = "ServiceUrls";
    public string CustomerClient { get; set; }
    public string PaymentProviderClient { get; set; }

    public string CustomerService { get; set; }
    public string  PaymentProviderService { get; set; }
}
