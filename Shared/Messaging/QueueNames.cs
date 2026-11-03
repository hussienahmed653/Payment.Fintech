namespace Shared.Messaging;

public static class QueueNames
{
    public const string UpdateCustomer = "customer-update";
    public const string InitiateDeposit = "payment-provider-deposit";
    public const string CreatePayment = "payment-request-create";
    public const string CreatePaymentRequest = "payment-request-created-queue";
    public const string SuccessPaymentRequest = "payment-request-succeeded-queue";
}
