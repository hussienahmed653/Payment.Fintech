using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum Currency
{
    USD,
    EUR,
    GBP,
    JPY,
    AUD,
    CAD,
    CHF,
    CNY,
    SEK,
    NZD,
    ZMW,
    XOF
}