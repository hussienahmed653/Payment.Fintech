using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum PayerType
{
    MMO,
    PawaPay
}