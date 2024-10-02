// Customer Model
using System.Text.Json.Serialization;

public class Customer
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;

    // one customer has many addresses
    [JsonIgnore]
    public List<Address> Addresses { get; set; }

    //one customer has many payments
    [JsonIgnore]
    public List<Payment> Payments { get; set; }

    // one customer has many orders
    // public List<Order> Ordes {get; set;}
}