using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Models;

[PrimaryKey(nameof(UserId), nameof(FriendId))]
public class Friendships
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ForeignKey("Users")]
    public Guid UserId { get; set; }

    public required Users User { get; init; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ForeignKey("Users")]
    public Guid FriendId { get; set; }

    public required Users Friend { get; init; }
}
