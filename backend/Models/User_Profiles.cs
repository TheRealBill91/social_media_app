using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

public class User_Profiles
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ForeignKey("Users")]
    public Guid UserId { get; set; }

    public required Users Users { get; init; }

    public required string PhotoURL { get; set; }
}
