using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Models;

[Table("friendship")]
[PrimaryKey(nameof(MemberId), nameof(FriendId))]
public class Friendships
{
    [Column("member_id")]
    public Guid MemberId { get; set; }

    [Column("friend_id")]
    public Guid FriendId { get; set; }
}
