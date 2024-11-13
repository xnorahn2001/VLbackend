// Create Product Dto
using System.ComponentModel.DataAnnotations;
public class CreateProductDto
{

    [Required(ErrorMessage = "Image is missing.")]
    public string Image { get; set; } = string.Empty;

    [Required(ErrorMessage = "Size is missing.")]
    public Size Size { get; set; }

    [Required(ErrorMessage = "Color is missing.")]
    public Color Color { get; set; }

    [Required(ErrorMessage = "Material is missing.")]
    public Material Material { get; set; }
    
    [Required(ErrorMessage = "Quantity is missing.")]
    public int Quantity { get; set; }
     
     
    // [Required(ErrorMessage = "Price is missing.")]

    // public decimal Price {get; set;}

}