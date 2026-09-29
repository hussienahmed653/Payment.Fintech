using System.Text.Json.Serialization;

namespace Customer.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CustomerStatus
{
    Pending,
    Active,
    Suspended,
    Inactive
}
[JsonConverter(typeof(JsonStringEnumConverter))]

public enum BusinessType
{
    Individual,
    SoleProprietorship,
    LLC,
    Corporation,
    NonProfit
}

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
