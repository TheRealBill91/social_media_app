using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Services;
using SocialMediaApp.Models;
using SocialMediaApp.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

[ApiController]
[ValidateModel]
[Route("api/posts/{postId}/comments")]
public class CommentController : Controller
{
    private readonly CommentService _commentService;

    private readonly UserManager<Member> _userManager;

    public CommentController(CommentService commentService, UserManager<Member> userManager)
    {
        _commentService = commentService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentDTO comment, Guid postId)
    {
        if (comment == null)
        {
            return NotFound();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return NotFound("No user id available");
        }

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound("Can't find the user");
        }

        var commentId = await _commentService.CreateComment(comment, user.Id, postId);
        return CreatedAtAction(nameof(CreateComment), new { id = commentId }, comment);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(Guid? id)
    {
        if (id == null)
        {
            Console.WriteLine(id);
            return NotFound();
        }

        var commentId = id.Value;

        var comment = await _commentService.GetComment(commentId);
        if (comment == null)
        {
            return NotFound("Comment does not exist");
        }

        return Ok(comment);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateComment(Guid? id, [FromBody] CommentDTO commentToUpdate)
    {
        if (id == null)
        {
            return NotFound("Comment id does not exist");
        }

        var commentId = id.Value;

        var comment = await _commentService.GetComment(commentId);

        if (comment == null)
        {
            return NotFound("Comment does not exists");
        }

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var commentAuthorId = comment.AuthorId;

        if (userId != commentAuthorId.ToString())
        {
            return Forbid();
        }

        await _commentService.UpdateCommentAsync(commentId, commentToUpdate);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(Guid? id)
    {
        if (id == null)
        {
            return NotFound("Comment id does not exist");
        }

        var commentId = id.Value;

        var comment = await _commentService.GetComment(commentId);

        if (comment == null)
        {
            return NotFound("Comment does not exist");
        }

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var commentAuthorId = comment.AuthorId;

        if (userId != commentAuthorId.ToString())
        {
            return Forbid();
        }

        var result = await _commentService.DeleteCommentAsync(commentId);
        if (result > 0)
        {
            return NoContent();
        }
        else
        {
            // If no rows were affected, the post with the given ID was not found.
            return NotFound();
        }
    }
}
