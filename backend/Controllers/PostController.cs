using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

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

        var result = await _postService.CreatePost(post, user.Id);
        if (result.Success)
        {
            return CreatedAtAction(nameof(GetPost), new { id = result.PostCreationId }, result);
        }
        else
        {
            return BadRequest(result.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllPosts(int page)
    {
        //TODO: check if user has permission to get all the posts
        var posts = await _postService.GetPosts(page);

        return Ok(posts);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
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
    [HttpPatch("{id:guid}")]
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

        var result = await _postService.UpdatePostAsync(postId, postToUpdate);

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
    [HttpDelete("{id:guid}")]
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
