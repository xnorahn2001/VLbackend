// Update Product Dto
using System.ComponentModel.DataAnnotations;
public class UpdateProductDto
{
    public string? Image { get; set; } = string.Empty;

    public Size? Size { get; set; }

    public Color? Color { get; set; }

  

    public Material? Material { get; set; }
    public int Quantity { get; set; }
}