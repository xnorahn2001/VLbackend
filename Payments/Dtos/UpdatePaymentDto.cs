using System.ComponentModel.DataAnnotations;
// update payment Dto
public class UpdatePaymentDto
{
    public PaymentMethod? PaymentMethod { get; set; }

    [StringLength(16, ErrorMessage = "CardNumber must be 15 or 16 characters.")]
    public string? CardNumber { get; set; }

}