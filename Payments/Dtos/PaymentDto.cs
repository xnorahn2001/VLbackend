// Payment Dto to return a specific data

public class PaymentDto
{
    public PaymentMethod paymentMethod { get; set; } 
    public int TotalPrice { get; set; }

    // one order has one payment
    public Guid OrderId { get; set; }
    public OrderDto Order { get; set; }

}