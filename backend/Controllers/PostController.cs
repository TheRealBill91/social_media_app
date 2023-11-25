using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Services;
using SocialMediaApp.Models;
using SocialMediaApp.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

[ApiController]
[ValidateModel]
[Route("api/posts")]
public class PostController : Controller
{
    private readonly PostService _postService;

    private readonly UserManager<Member> _userManager;

    public PostController(PostService postService, UserManager<Member> userManager)
    {
        _postService = postService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostDTO post)
    {
        if (post == null)
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

        var postId = await _postService.CreatePost(post, user.Id);
        return CreatedAtAction(nameof(CreatePost), new { id = postId }, post);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPost(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var postId = id.Value;

        var post = await _postService.GetPost(postId);
        if (post == null)
        {
            return NotFound("Post does not exist");
        }
        return Ok(post);
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdatePost(Guid? id, [FromBody] PostDTO postToUpdate)
    {
        if (id == null)
        {
            return NotFound();
        }

        var postId = id.Value;

        var post = await _postService.GetPost(postId);

        if (post == null)
        {
            return NotFound("Post does not exists");
        }

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var postAuthorId = post.AuthorId;

        if (userId != postAuthorId.ToString())
        {
            return Forbid();
        }

        await _postService.UpdatePostAsync(postId, postToUpdate);
        return Ok();
    }

    [Authorize]
    [HttpPatch("{id}")]
    public async Task<IActionResult> DeletePost(Guid? id)
    {
        if (id == null)
        {
            return NotFound("Post id does not exist");
        }

        var postId = id.Value;

        var post = await _postService.GetPost(postId);

        if (post == null)
        {
            return NotFound();
        }

        string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var postAuthorId = post.AuthorId;

        if (userId != postAuthorId.ToString())
        {
            return Forbid();
        }

        var result = await _postService.DeletePostAsync(postId);
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
