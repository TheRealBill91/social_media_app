using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class ResetPasswordDTO
{
    [Required(ErrorMessage = "New password is required")]
    [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 8)]
    [DataType(DataType.Password)]
    public required string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirming new password is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public required string NewPasswordConfirmation { get; set; }

    public required string Code { get; set; }

    public required Guid PasswordResetUserId { get; set; }
}
