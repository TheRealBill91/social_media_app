using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

[Table("member_profile")]
public class Member_Profiles
{
    [Key]
    [Column("member_id")]
    public Guid MemberId { get; set; }

    [Column("photo_url")]
    public string? PhotoURL { get; set; }

    [Column("bio")]
    public string? Bio { get; set; }

    [Column("location")]
    public string? Location { get; set; }

    [Column("url")]
    public string? URL { get; set; }

    [Required]
    [Column("created_at")]
    public required DateTime CreatedAt { get; set; }

    [Required]
    [Column("updated_at")]
    public required DateTime UpdatedAt { get; set; }

    [Required]
    [Column("deleted_at")]
    public required DateTime DeletedAt { get; set; }
}
