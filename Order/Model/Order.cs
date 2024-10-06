//Order  Model
using System.Text.Json.Serialization;

public class Order
{
    public Guid OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }


    // one user has many orders
    public Guid UserId { get; set; } //FK
    [JsonIgnore]
    public User User { get; set; }

    // one order has one orderDetailses
    [JsonIgnore]
    public List<OrderDetails> OrderDetails { get; set; }

    // one order has one shipment
    public Guid ShipmentId { get; set; }
    [JsonIgnore]
    public Shipment Shipment { get; set; }

    // one order one payment
    public Guid PaymentId { get; set; }
    [JsonIgnore]
    public Payment Payment { get; set; }



}