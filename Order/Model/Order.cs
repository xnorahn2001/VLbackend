//Order  Model
using System.Text.Json.Serialization;

public class Order
{
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
    public int TotalPrice { get; set; }

    // one user has many orders
    public Guid UserId { get; set; } //FK
    public User User { get; set; }

    // one order has one orderDetailses
    public List<OrderDetails> OrderDetails { get; set; }

    // one order has one shipment
    public Shipment Shipment { get; set; }

    // one order one payment
    public Payment Payment { get; set; }



}