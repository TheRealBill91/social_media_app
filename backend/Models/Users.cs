using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaApp.Models;

public class Users
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [MinLength(5, ErrorMessage = "Minimum length of 5 characters required.")]
    [MaxLength(15, ErrorMessage = "Maximum length of 15 characters exceeded.")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public required string Email { get; set; }

    public DateTime LastActive { get; set; }
};
