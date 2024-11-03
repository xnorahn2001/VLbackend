// public enum for t-shirt meterial
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Material
{
    Coton,
    Crepe,
    Silk,
    Linen,
    Polyester,
    Wool,
    Cashmere,
    Leather,
    Mesh,
    Knit
}