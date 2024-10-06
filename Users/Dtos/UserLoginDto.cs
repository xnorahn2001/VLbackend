// User Dto log in 
using System.ComponentModel.DataAnnotations;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(50, ErrorMessage = "Password must be between 6 and 50 characters.", MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}