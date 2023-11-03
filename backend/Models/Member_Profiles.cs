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
    public required string PhotoURL { get; set; }
}
