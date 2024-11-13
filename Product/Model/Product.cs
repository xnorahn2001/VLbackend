
using System.Text.Json.Serialization;

public class Product
{
    public Guid ProductId { get; set; }
    public string Image { get; set; } = string.Empty;
    public decimal Price { get; } =89;
    public Size Size { get; set; }
    public Color Color { get; set; }
    public Material Material { get; set; }
    public int Quantity { get; set; }

    // one Product has one orderDetails => one product could be in many orderDetail
    [JsonIgnore]
    public List<OrderDetails> OrderDetailses { get; set; }
}