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
    [HttpGet("{id}")]
    public async Task<IActionResult> GetComment(Guid? id)
    {
        // TODO: check if user has permission to get the comment

        if (id == null)
        {
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

        var result = await _commentService.CreateComment(comment, user.Id, postId);
        Console.WriteLine(result.CommentCreationId);
        if (result.Success)
        {
            return CreatedAtAction(
                nameof(GetComment),
                new { postId, id = result.CommentCreationId },
                result
            );
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllComments(int page, Guid? postId)
    {
        // TODO: check if user has permission to get all the comments
        if (postId == null)
        {
            return BadRequest();
        }

        var comments = await _commentService.GetComments(page, postId.Value);

        return Ok(comments);
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

        var result = await _commentService.UpdateCommentAsync(commentId, commentToUpdate);
        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
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
        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }
}
