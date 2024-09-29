// Customer Dto to update existing customer

using System.ComponentModel.DataAnnotations;

public class UpdateCustomerDto
{
    [StringLength(55, ErrorMessage = "FirstName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string? FirstName { get; set; }

    [StringLength(55, ErrorMessage = "LastName must be between 3 and 55 characters.", MinimumLength = 3)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, ErrorMessage = "Password must be between 6 and 50 characters.", MinimumLength = 6)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    [StringLength(10, ErrorMessage = "Phone number must be exactly 10 characters.", MinimumLength = 10)]
    public string? Phone { get; set; }
}