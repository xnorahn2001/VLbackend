// Adress Dto to update an existing Adress for the customer
using System.ComponentModel.DataAnnotations;

public class UpdateAddressDto
{
    [StringLength(55, ErrorMessage = "AddressName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string? AddressName { get; set; }

    [StringLength(55, ErrorMessage = "StreetName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string? StreetName { get; set; }

    [StringLength(20, ErrorMessage = "StreetNumber must be between 3 and 20 characters.", MinimumLength = 3)]
    public string? StreetNumber { get; set; }

    [StringLength(55, ErrorMessage = "City must be between 3 and 55 characters.", MinimumLength = 3)]
    public string? City { get; set; }


    [StringLength(55, ErrorMessage = "State must be between 3 and 55 characters.", MinimumLength = 3)]
    public string? State { get; set; }

}
