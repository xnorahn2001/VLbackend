// public enum for t-shirt color 
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Color
{
    Red,
    Blue,
    Black,
    White,
    Green
}