public class Shipment
{
    public Guid ShipmentId { get; set; }
    public DateTime ShipmentDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Status Status { get; set; }
    public int TrackingNumber { get; set; } 

    // one order has one shipment
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

}