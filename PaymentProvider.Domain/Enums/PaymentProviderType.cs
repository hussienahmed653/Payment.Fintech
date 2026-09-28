using System.Text.Json.Serialization;

namespace PaymentProvider.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentProviderType
{
    PawaPay,
    MMO,
}
