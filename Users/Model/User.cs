// User Model
using System.Text.Json.Serialization;

public class User
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool IsAdmin { get; set; } = false;

    // one user has many addresses
    [JsonIgnore]
    public List<Address> Addresses { get; set; }

    //one user has many payments
    [JsonIgnore]
    public List<Payment> Payments { get; set; }

    // one user has many orders
    public List<Order> Orders {get; set;}
}