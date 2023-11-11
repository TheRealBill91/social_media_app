using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class SignInDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
