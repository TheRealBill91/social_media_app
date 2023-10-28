using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace DotNetSQL.Models;

public class Posts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }

    public DateOnly Created { get; set; }

    public DateOnly Modified { get; set; }

    [ForeignKey("Users")]
    public Guid AuthorId { get; set; }

    public required Users Users { get; init; }
}
