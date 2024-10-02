// Payment Dto to return a specific data

public class PaymentDto
{
    public PaymentMethod paymentMethod { get; set; } 
    public required string CardNumber { get; set; }
    public decimal TotalPrice { get; set; }

    // one customer has many payments
    public Guid CustomerId { get; set; }
    public CustomerDto Customer { get; set; }
}