using System.Text.Json.Serialization;

public class Shipment
{
    public Guid ShipmentId { get; set; }
    public DateTime ShipmentDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Status Status { get; set; }
    public int TrackingNumber { get; set; } 

    // one order has one shipment
    public Guid OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }

}