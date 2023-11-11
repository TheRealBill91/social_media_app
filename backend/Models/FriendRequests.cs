using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Models;

[PrimaryKey(nameof(RequesterId), nameof(ReceiverId))]
public class FriendRequests
{
    public Guid RequesterId { get; set; }

    public Guid ReceiverId { get; set; }

    public FriendRequestStatus Status { get; set; }
}
