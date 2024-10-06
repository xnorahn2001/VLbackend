public class OrderDto
{
    public int Quantity { get; set; }
    public int TotalPrice { get; set; }
    // one user has many orders
    public Guid UserId { get; set; }
    public UserDto User { get; set; }

    // one order has many orderDetails
    public Guid OrderDetailsId { get; set; }
    public OrderDetailsDto OrderDetails { get; set; }

}