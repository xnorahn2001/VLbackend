using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Status
{
    Delivered,
    Canceled,
    Shipped,
    Delayed
}