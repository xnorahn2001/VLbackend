// Payment Model
using System.Text.Json.Serialization;

public class Payment
{
    public Guid PaymentId { get; set; }

    public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.ApplePay;
    public string CardNumber { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; } = 0;

    // one customer has many payments
    public Guid CustomerId { get; set; } //FK

    public Customer Customer { get; set; }

}