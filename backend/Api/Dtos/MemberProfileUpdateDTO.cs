using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.ValidationAttributes;

[Keyless]
public class MemberProfileUpdateDTO
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(50, ErrorMessage = "Maximum length of 50 characters exceeded.")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Username is required.")]
    [MinLength(5, ErrorMessage = "Minimum length of 5 characters required.")]
    [MaxLength(15, ErrorMessage = "Maximum length of 15 characters exceeded.")]
    [DataType(DataType.Text)]
    public string UserName { get; set; } = null!;

    // url for profile photo
    [StringLength(
        200,
        ErrorMessage = "Maximum URL length of 200 characters exceeded."
    )]
    [OptionalUrl(ErrorMessage = "Invalid URL format.")]
    [DataType(DataType.Text)]
    public string? Photo_url { get; set; }

    [MaxLength(
        500,
        ErrorMessage = "Maximum bio length of 500 characters exceeded."
    )]
    [DataType(DataType.Text)]
    public string? Bio { get; set; }

    [MaxLength(
        100,
        ErrorMessage = "Maximum location length of 100 characters exceeded."
    )]
    public string? Location { get; set; }

    // url for linking to another website for example
    [OptionalUrl(ErrorMessage = "Invalid URL format.")]
    [MaxLength(
        200,
        ErrorMessage = "Maximum URL length of 200 characters exceeded."
    )]
    public string? Url { get; set; }
}
