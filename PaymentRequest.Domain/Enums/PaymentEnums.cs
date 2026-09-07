namespace PaymentRequest.Domain.Enums;


public enum PaymentStatus
{
    Created,
    Processing,
    Successful,
    Failed
}

public enum PaymentCurrency
{
    EGP,
    USD,
    EUR,
    GBP,
    JPY,
    AUD,
    CAD,
    CHF,
    CNY,
    SEK,
    NZD
}