namespace PaymentProvider.Application.Common.Contracts.Deposit;

public record InitiateDepositResponse(
    string DepositId,
    Status Status,
    DateTime? Created,
    FailureReason? FailureReason
);

public record FailureReason(
    FailureCode FailureCode,
    string FailureMessage
);
public enum Status
{
    ACCEPTED,
    REJECTED,
    DUPLICATE_IGNORED
}
public enum FailureCode
{
    NO_AUTHENTICATION,
    AUTHENTICATION_ERROR,
    AUTHORISATION_ERROR,
    HTTP_SIGNATURE_ERROR,
    INVALID_INPUT,
    MISSING_PARAMETER,
    UNSUPPORTED_PARAMETER,
    INVALID_PARAMETER,
    DUPLICATE_METADATA_FIELD,
    DEPOSITS_NOT_ALLOWED,
    INVALID_PHONE_NUMBER,
    INVALID_AMOUNT,
    AMOUNT_OUT_OF_BOUNDS,
    INVALID_CURRENCY,
    INVALID_PROVIDER,
    PROVIDER_TEMPORARILY_UNAVAILABLE,
    UNKNOWN_ERROR
}