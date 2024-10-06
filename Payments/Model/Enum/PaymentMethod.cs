// public enum for the payment method
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentMethod
{
    CreditCard,
    ApplePay,
    Cash
}
