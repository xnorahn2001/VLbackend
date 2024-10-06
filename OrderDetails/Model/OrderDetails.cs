//Order Details Model
using System.Text.Json.Serialization;

public class OrderDetails
{
    public Guid OrdersDetailesId { get; set; }              
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }

    // one order has one order detailses 
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    // one product has many order detailses 
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

}