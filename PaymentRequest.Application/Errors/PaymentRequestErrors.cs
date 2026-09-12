namespace Payment.Fintech.Domain.Errors;

public record PaymentRequestErrors
{
    public static readonly Error PaymentProcessingFailed =
        new("paymentRequest.PaymentProcessingFailed",
            "Payment processing failed.",
            StatusCodes.Status400BadRequest);
    public static readonly Error PaymentRequestNotFound =
        new("paymentRequest.PaymentRequestNotFound",
            "Payment request not found.",
            StatusCodes.Status404NotFound);
    public static readonly Error InvalidStatusForProcessing =
        new ("paymentRequest.InvalidStatusForProcessing",
             "This payment request cannot be processed because it is already processed, failed, or currently under processing.",
             StatusCodes.Status409Conflict);
    public static readonly Error ZeroRowsAffected =
        new("paymentRequest.ZeroRowsAffected",
            "No Rows Affected. Expected (1) Row Affected",
            StatusCodes.Status400BadRequest);
    public static readonly Error MultibleRowsAffected =
        new("paymentRequest.MultibleRowsAffected",
            "Multible Rows Affected. Expected (1) Row Affected",
            StatusCodes.Status400BadRequest);
}
