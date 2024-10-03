// Adress Model 
using System.Text.Json.Serialization;

public class Address
{

    public Guid AddressId { get; set; }
    public string AddressName { get; set; } = string.Empty;
    public string StreetNumber { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    // one user has many addresses
    public Guid UserId { get; set; } //FK

    public User User { get; set; }

}