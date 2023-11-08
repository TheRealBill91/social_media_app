using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Models;

[Table("friend_request")]
[PrimaryKey(nameof(RequesterId), nameof(ReceiverId))]
public class Friend_Requests
{
    [Column("requester_id")]
    public Guid RequesterId { get; set; }

    [Column("receiver_id")]
    public Guid ReceiverId { get; set; }

    [Column("status")]
    public FriendRequestStatus Status {get; set;}
}
