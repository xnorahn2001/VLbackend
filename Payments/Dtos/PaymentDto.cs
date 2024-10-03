// Payment Dto to return a specific data

public class PaymentDto
{
    public PaymentMethod paymentMethod { get; set; } 
    public required string CardNumber { get; set; }
    public decimal TotalPrice { get; set; }

    // one user has many payments
    public Guid UserId { get; set; }
    public UserDto User { get; set; }
}