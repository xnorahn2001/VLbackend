using System.ComponentModel.DataAnnotations;

public class CreateOrderDto
{
    [Required]
    public Guid UserId { get; set; } //FK

    [Required]
    public Guid OrderDetailsId { get; set; } //FK

}