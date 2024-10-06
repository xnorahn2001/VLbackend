// Product Dto to return specefic data
public class ProductDto
{
    public Size Size { get; set; }
    public Color Color { get; set; }
    public Material Material { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}