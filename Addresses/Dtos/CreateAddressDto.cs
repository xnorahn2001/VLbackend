// Adress Dto to create new Adress for the customer
using System.ComponentModel.DataAnnotations;

public class CreateAddressDto
{
    [Required(ErrorMessage = "AddressName is missing.")]
    [StringLength(55, ErrorMessage = "AddressName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string AddressName { get; set; } = string.Empty;

    [Required(ErrorMessage = "StreetName is missing.")]
    [StringLength(55, ErrorMessage = "StreetName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string StreetName { get; set; } = string.Empty;

    [Required(ErrorMessage = "StreetNumber is missing.")]
    [StringLength(20, ErrorMessage = "StreetNumber must be between 3 and 20 characters.", MinimumLength = 3)]
    public string StreetNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "City is missing.")]
    [StringLength(55, ErrorMessage = "City must be between 3 and 55 characters.", MinimumLength = 3)]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is missing.")]
    [StringLength(55, ErrorMessage = "State must be between 3 and 55 characters.", MinimumLength = 3)]
    public string State { get; set; } = string.Empty;

    [Required]
    public Guid CustomerId { get; set; } //FK


}