using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class SignUpDTO
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(3, ErrorMessage = "Minimum length of 3 characters required.")]
    [MaxLength(20, ErrorMessage = "Maximum length of 20 characters exceeded.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [MinLength(3, ErrorMessage = "Minimum length of 3 characters required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public required string PasswordConfirmation { get; set; }
}
