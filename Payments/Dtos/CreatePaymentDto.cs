// Create Payment Model
using System.ComponentModel.DataAnnotations;

public class CreatePaymentDto
{
    [Required(ErrorMessage = "PaymentMethod is missing.")]
    public PaymentMethod PaymentMethod { get; set; } 
    
    [Required(ErrorMessage = "CardNumber is missing.")]
    [StringLength(50, ErrorMessage = "CardNumber must be 15 or 16 characters.", MinimumLength = 6)]
    public string CardNumber { get; set; } = string.Empty;

    [Required]
    public Guid UserId { get; set; } //FK
}