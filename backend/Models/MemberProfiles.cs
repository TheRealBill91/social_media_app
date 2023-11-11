using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

public class MemberProfiles
{
    [Key]
    public Guid MemberId { get; set; }

    public string? PhotoURL { get; set; }

    public string? Bio { get; set; }

    public string? Location { get; set; }

    public string? URL { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
