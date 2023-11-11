using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class SignUpDTO
{
    [Required(ErrorMessage = "Username is required.")]
    [MinLength(5, ErrorMessage = "Minimum length of 5 characters required.")]
    [MaxLength(15, ErrorMessage = "Maximum length of 15 characters exceeded.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Password confirmation is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public required string ConfirmPassword { get; set; }
}
