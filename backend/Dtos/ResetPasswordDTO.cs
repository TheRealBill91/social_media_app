using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class ResetPasswordDTO
{
    [Required(ErrorMessage = "Password is required")]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = "New password is required")]
    [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
    public required string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirming new password is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public required string ConfirmNewPassword { get; set; }
}
