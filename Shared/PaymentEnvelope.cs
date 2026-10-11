using Shared.Sections;

namespace Shared;

public class PaymentEnvelope
{
    public required PaymentRequestSection payment { get; set; }
    public CustomerRequestSection? customer { get; set; }
    public TransactionSection? transaction { get; set; }
    public ProviderSection? provider { get; set; }
}