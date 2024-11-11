// public enum for t-shirt meterial
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Material
{
    Plastic,
    Glass,
    Metal,
    Titanium,
    Flexon,
    Aluminum,
    Wooden,
    Carbon,
    Rimless
}