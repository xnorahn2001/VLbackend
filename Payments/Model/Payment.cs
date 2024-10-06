// Payment Model
using System.Text.Json.Serialization;

public class Payment
{
    public Guid PaymentId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.ApplePay;
    public string CardNumber { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; } = 0;

    // one user has many payments
    public Guid UserId { get; set; } //FK
    [JsonIgnore]
    public User User { get; set; }

    // one order one payment
    public Guid OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }

}