using MassTransit;

namespace PaymentProvider.Infrastructure.Sagas;

public class PaymentState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid PaymentProviderFlowId { get; set; }
    public string? CollectionMethod { get; set; }
    public DateTime InitiatedAt { get; set; }
    public DateTime? TransactionCreatedAt { get; set; }
    public DateTime? PaymentRequestedAt { get; set; }
    public DateTime? PayLinkCreatedAt { get; set; }
    public DateTime? TransactionUpdatedAt { get; set; }
    public DateTime? PaymentStatusChangedAt { get; set; }
    public DateTime? IpnOrderGeneratedAt { get; set; }
}
