// Payment Model
using System.Text.Json.Serialization;

public class Payment
{
    public Guid PaymentId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.ApplePay;
    public string CardNumber { get; set; } = string.Empty;
    public int TotalPrice { get; set; } = 0;

    // one user has many payments
    public Guid UserId { get; set; } //FK
    public User User { get; set; }

    // one order one payment
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

}