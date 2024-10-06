// public enum for t-shit size
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Size
{
    Small,
    Large,
    Medium
}