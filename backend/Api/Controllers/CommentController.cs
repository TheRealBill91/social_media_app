using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Controllers;

[Route("api/posts/{postId}/comments")]
public class CommentController : BaseApiController
{
    private readonly CommentService _commentService;

    private readonly PostService _postService;

    private readonly FriendshipService _friendshipService;

    private readonly UserManager<Member> _userManager;

    public CommentController(
        CommentService commentService,
        PostService postService,
        FriendshipService friendshipService,
        UserManager<Member> userManager
    )
    {
        _commentService = commentService;
        _postService = postService;
        _friendshipService = friendshipService;
        _userManager = userManager;
    }

    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetComment(Guid? id)
    {
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

        var commentAuthorId = comment.AuthorId;

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

        var friendship = await _friendshipService.GetFriendship(
            Guid.Parse(userId),
            commentAuthorId
        );

        // return 403 if users aren't friends or the logged in user is not the author
        // of the post
        if (friendship == null && comment.AuthorId.ToString() != userId)
        {
            return Forbid();
        }

        return Ok(comment);
    }

    [EnableRateLimiting("createResourceSlidingWindow")]
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateComment(
        [FromBody] CommentDTO comment,
        Guid postId
    )
    {
        if (comment == null)
        {
            return NotFound();
        }

        var post = await _postService.GetPost(postId);

        if (post == null)
        {
            return BadRequest("Post does not exist");
        }

        var postAuthorId = post.AuthorId;

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

        var friendship = await _friendshipService.GetFriendship(
            Guid.Parse(userId),
            postAuthorId
        );

        // return 403 if users aren't friends or the logged in user is not the author
        // of the post
        if (friendship == null && post.AuthorId.ToString() != userId)
        {
            return Forbid();
        }

        var result = await _commentService.CreateComment(
            comment,
            user.Id,
            postId
        );
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

    [EnableRateLimiting("getResourceSlidingWindow")]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllComments(int page, Guid? postId)
    {
        if (postId == null)
        {
            return BadRequest();
        }

        var post = await _postService.GetPost(postId.Value);

        if (post == null)
        {
            return BadRequest("Post does not exist");
        }

        var postAuthorId = post.AuthorId;

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

        var friendship = await _friendshipService.GetFriendship(
            Guid.Parse(userId),
            postAuthorId
        );

        // return 403 if users aren't friends or the logged in user is not the author
        // of the post
        if (friendship == null && post.AuthorId.ToString() != userId)
        {
            return Forbid();
        }

        var comments = await _commentService.GetComments(page, postId.Value);

        return Ok(comments);
    }

    [EnableRateLimiting("updateResourceSlidingWindow")]
    [Authorize]
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateComment(
        Guid? id,
        [FromBody] CommentDTO commentToUpdate
    )
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

        var result = await _commentService.UpdateComment(
            commentId,
            commentToUpdate
        );
        if (result.Success)
        {
            return Ok(result.Message);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [EnableRateLimiting("deleteResourceSlidingWindow")]
    [Authorize]
    [HttpDelete("{id:guid}")]
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

        var result = await _commentService.DeleteComment(commentId);
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
