using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace DotNetSQL.Models;

[PrimaryKey(nameof(RequesterId), nameof(ReceiverId))]
public class Friend_Requests
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ForeignKey("Users")]
    public Guid RequesterId { get; set; }

    public required Users Requester { get; init; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ForeignKey("Users")]
    public Guid ReceiverId { get; set; }

    public required Users Receiver { get; init; }
}
