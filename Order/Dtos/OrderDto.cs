using System.Text.Json.Serialization;

public class OrderDto
{
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    // one user has many orders
    public Guid UserId { get; set; }
    public UserDto User { get; set; }

    // one order has many orderDetails
    public Guid OrderDetailsId { get; set; }
    public List<OrderDetailsDto> OrderDetailses { get; set; }

    public PaymentDto Payment { get; set; }


}