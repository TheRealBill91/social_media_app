using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace DotNetSQL.Models;

public class Post_Upvotes
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public required DateTime Timestamp { get; set; }

    [ForeignKey("Users")]
    public Guid AuthorId { get; set; }

    public required Users Users { get; init; }

    [ForeignKey("Posts")]
    public Guid PostId { get; set; }

    public required Posts Posts { get; init; }
}
