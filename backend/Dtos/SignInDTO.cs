using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class SignInDTO
{
    [Required(ErrorMessage = "Email or Username is required.")]
    public required string EmailOrUsername { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    public bool? RememberMe { get; set; }
}
