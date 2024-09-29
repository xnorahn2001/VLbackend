// Adress Model 
public class Address
{
    public Guid AdressId { get; set; }
    public Guid CustomerId { get; set; }
    public string AddressName { get; set; } = string.Empty;
    public string StreetNumber { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

}