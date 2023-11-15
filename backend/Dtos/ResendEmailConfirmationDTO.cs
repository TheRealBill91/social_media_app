using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class ResendEmailConfirmationDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
