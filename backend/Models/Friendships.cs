using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Models;

[PrimaryKey(nameof(MemberId), nameof(FriendId))]
public class Friendships
{
    public Guid MemberId { get; set; }

    public Guid FriendId { get; set; }
}
