using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class ExternalLoginConfirmationDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
