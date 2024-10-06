public class ShipmentDto
{
    public DateTime ShipmentDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public Status Status { get; set; }
    public int TrackingNumber { get; set; }
    public Guid OrderId { get; set; }
    public OrderDto Order { get; set; }
}