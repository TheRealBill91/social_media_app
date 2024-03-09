using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class RequestForgotUsernameDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [MinLength(3, ErrorMessage = "Minimum length of 3 characters required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    [DataType(DataType.Text)]
    public string Email { get; set; } = null!;
}
