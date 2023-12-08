using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class RequestPasswordResetDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [DataType(DataType.Text)]
    public string Email { get; set; } = null!;
}
