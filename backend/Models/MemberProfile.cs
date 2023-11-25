using System.ComponentModel.DataAnnotations;

namespace SocialMediaApp.Models;

public class MemberProfile
{
    [Key]
    public Guid MemberId { get; set; }

    public string? PhotoURL { get; set; }

    public string? Bio { get; set; }

    public string? Location { get; set; }

    public string? URL { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
