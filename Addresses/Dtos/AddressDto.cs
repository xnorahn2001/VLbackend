// Adress Dto to return specific data 

public class AddressDto
{
    public string AddressName { get; set; }
    public string StreetNumber { get; set; }
    public string StreetName { get; set; }
    public string City { get; set; }
    public string State { get; set; }

    // one customer has many addresses
    public Guid CustomerId { get; set; }
    public CustomerDto Customer { get; set; }

}