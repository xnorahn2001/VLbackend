// Customer Dto to create new customer
using System.ComponentModel.DataAnnotations;
public class CreateCustomerDto
{

    [Required(ErrorMessage = "FirstName is missing.")]
    [StringLength(55, ErrorMessage = "FirstName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "LastName is missing.")]
    [StringLength(55, ErrorMessage = "LastName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, ErrorMessage = "Password must be between 6 and 50 characters.", MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required.")]
    [StringLength(10, ErrorMessage = "Phone number must be exactly 10 characters.", MinimumLength = 10)]
    public string Phone { get; set; } = string.Empty;
}