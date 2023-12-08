using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

[ApiController]
[ValidateModel]
[Route("/api/posts/{postId:guid}")]
public class PostUpvoteController : Controller
{
    private readonly UserManager<Member> _userManager;

    private readonly PostUpvoteService _postUpvoteService;

    public PostUpvoteController(
        UserManager<Member> userManager,
        PostUpvoteService postUpvoteService
    )
    {
        _userManager = userManager;

        _postUpvoteService = postUpvoteService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> TogglePostUpvote(Guid? postId)
    {
        if (postId == null)
        {
            return BadRequest();
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

        //check for existing post upvote for the current user
        var postUpvote = await _postUpvoteService.GetPostUpvote(postId, Guid.Parse(userId));

        if (postUpvote != null)
        {
            // delete post upvote and return 200
            var upvoteDeletionResponse = await _postUpvoteService.DeletePostUpvote(
                postId,
                Guid.Parse(userId)
            );

            if (upvoteDeletionResponse.Success)
            {
                return Ok(upvoteDeletionResponse.Message);
            }
            else
            {
                return BadRequest(upvoteDeletionResponse.Message);
            }
        }

        // create the post upvote since it is not null
        var upvoteCreationReponse = await _postUpvoteService.CreatePostUpvote(
            postId,
            Guid.Parse(userId)
        );

        if (upvoteCreationReponse.Success)
        {
            return Ok(upvoteCreationReponse.Message);
        }
        else
        {
            return BadRequest(upvoteCreationReponse.Message);
        }
    }
}
