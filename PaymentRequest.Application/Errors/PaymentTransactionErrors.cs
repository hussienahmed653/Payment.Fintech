namespace PaymentRequest.Application.Errors;

public static class PaymentTransactionErrors
{
    public static readonly Error ConcurrentPaymentProcessing =
        new("PaymentTransaction.ConcurrentProcessing",
            "A payment processing request with this key is currently underway. Please wait a moment and try again.",
            StatusCodes.Status409Conflict);
    public static readonly Error IdempotencyKeyMismatch =
        new("PaymentTransaction.IdempotencyKeyMismatch",
            "This Idempotency-Key has already been used for a different payment request.",
            StatusCodes.Status409Conflict);
    public static readonly Error DuplicateSubmissionNotAllowed =
        new("PaymentTransaction.DuplicateSubmissionNotAllowed",
            "This payment has already been submitted.",
            StatusCodes.Status409Conflict);
    public static Error PaymentFailed(this string FailuerResponse) =>
        new("PaymentTransaction.PaymentFailed",
            $"{FailuerResponse}",
            StatusCodes.Status400BadRequest);
}