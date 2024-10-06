using System.ComponentModel.DataAnnotations;

public class CreateOrderDto
{
    [Required(ErrorMessage = "UserId is missing.")]
    public Guid UserId { get; set; } //FK

    [Required(ErrorMessage = "OrderDetailsId is missing.")]
    public List<CreateOrderDetailsDto> OrderDetailses { get; set; } //FK
    [Required(ErrorMessage = "PaymentMethod is missing.")]
    public PaymentMethod? PaymentMethod { get; set; } = null;

    [Required(ErrorMessage = "CardNumber is missing.")]
    public string CardNumber { get; set; } = string.Empty;


}