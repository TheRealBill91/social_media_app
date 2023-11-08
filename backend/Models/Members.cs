using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMediaApp.Models;

[Table("member")]
public class Members
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    [Column("first_name")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    [Column("last_name")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [MinLength(5, ErrorMessage = "Minimum length of 5 characters required.")]
    [MaxLength(15, ErrorMessage = "Maximum length of 15 characters exceeded.")]
    [Column("username")]
    public required string UserName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [Column("email")]
    public required string Email { get; set; }

    [Required]
    [Column("created_at")]
    public required DateTime CreatedAt { get; set; }

    [Required]
    [Column("updated_at")]
    public required DateTime UpdatedAt { get; set; }

    [Required]
    [Column("deleted_at")]
    public required DateTime DeletedAt { get; set; }
};
