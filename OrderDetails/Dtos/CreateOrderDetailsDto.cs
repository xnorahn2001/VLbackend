// CreateOrderDto create new order detailes that contain at least one product
using System.ComponentModel.DataAnnotations;
public class CreateOrderDetailsDto
{
    public int Quantity { get; set; }
    [Required]
    public Guid ProductId { get; set; } //FK
}