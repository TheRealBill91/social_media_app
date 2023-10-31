using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Components.Web;

namespace SocialMediaApp.Models;

public class Posts
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(100, ErrorMessage = "Maximum length of 100 characters exceeded.")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Content is required.")]
    [MaxLength(5000, ErrorMessage = "Maximum length of 5000 characters exceeded.")]
    public required string Content { get; set; }

    public DateOnly Created { get; set; }

    public DateOnly Modified { get; set; }

    [ForeignKey("Users")]
    public Guid AuthorId { get; set; }

    public required Users Users { get; init; }
}
