using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SocialMediaApp.Models;

public class Member : IdentityUser<Guid>
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public string LastName { get; set; } = null!;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int EmailConfirmationSentCount { get; set; }
    public DateTime LastEmailConfirmationSentDate { get; set; }

    public int PasswordResetEmailSentCount { get; set; }

    public DateTime LastPasswordResetEmailSentDate { get; set; }

    public int UsernameRequestEmailSentCount { get; set; }

    public DateTime LastUsernameRequestEmailSentDate { get; set; }

    public DateTime LastUsernameUpdateDate { get; set; }
};
