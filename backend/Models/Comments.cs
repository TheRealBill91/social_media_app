using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

public class Comments
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public required DateOnly Created { get; set; }

    [ForeignKey("Users")]
    public Guid AuthorId { get; set; }

    public required Users Users { get; init; }

    [ForeignKey("Posts")]
    public Guid PostId { get; set; }

    public required Posts Posts { get; init; }
}
