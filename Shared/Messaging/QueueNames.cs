namespace Shared.Messaging;

public static class QueueNames
{
    public const string UpdateCustomer = "customer-update";
    public const string InitiateDeposit = "payment-provider-deposit";
    public const string CreatePayment = "payment-request-create";
}
