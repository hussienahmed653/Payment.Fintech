namespace Payment.Fintech.Domain.Errors;

public record PaymentRequestErrors
{
    public static readonly Error PaymentProcessingFailed =
        new("PAYMENT_PROCESSING_FAILED", "Payment processing failed.", StatusCodes.Status400BadRequest);
    public static readonly Error PaymentRequestNotFound =
        new("PAYMENT_REQUEST_NOT_FOUND", "Payment request not found.", StatusCodes.Status404NotFound);
    public static readonly Error InvalidStatusForProcessing =
        new ("INVALID_STATUS_FOR_PROCESSING", "This payment request cannot be processed because it is already processed, failed, or currently under processing.", StatusCodes.Status409Conflict);
    public static readonly Error ZeroRowsAffected =
        new("NO_ROWS_AFFECTED", "No Rows Affected. Expected (1) Row Affected", StatusCodes.Status400BadRequest);
    public static readonly Error MultibleRowsAffected =
        new("MULTIBLE_ROWS_AFFECTED", "Multible Rows Affected. Expected (1) Row Affected", StatusCodes.Status400BadRequest);
}
