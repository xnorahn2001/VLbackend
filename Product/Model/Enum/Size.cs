// public enum for t-shit size
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Size
{
    S,
    L,
    M,
    XL,
    XXL,
    Onesize

}