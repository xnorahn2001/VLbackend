// Create Payment Model
using System.ComponentModel.DataAnnotations;

public class CreatePaymentDto
{
    [Required(ErrorMessage = "PaymentMethod is missing.")]
    public PaymentMethod PaymentMethod { get; set; } 
    
    [Required(ErrorMessage = "CardNumber is missing.")]
    [StringLength(50, ErrorMessage = "CardNumber must be 15 or 16 characters.", MinimumLength = 6)]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "TotalPraice is missing.")]
    [Range(30, 9999.99, ErrorMessage = "TotalPraice must be between 1 and 4 characters.")]
    public decimal TotalPrice { get; set; } = 0;

    [Required]
    public Guid CustomerId { get; set; } //FK
}