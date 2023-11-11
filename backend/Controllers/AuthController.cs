using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<Members> _signInManager;
    private readonly UserManager<Members> _userManager;

    private readonly AuthService _authService;

    public AuthController(
        SignInManager<Members> signInManager,
        UserManager<Members> userManager,
        AuthService authService
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost("signin")]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] SignInDTO form)
    {
        if (User.Identity.IsAuthenticated)
        {
            return BadRequest("User is already authenticated");
        }

        // Find the user by email
        var user = await _userManager.FindByEmailAsync(form.Email);

        if (user == null || user.UserName == null)
        {
            return BadRequest("Invalid email and/or password");
        }

        Console.WriteLine("User login enabled? " + user.LockoutEnabled);

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            form.Password,
            true, // For 'Remember me' functionality
            lockoutOnFailure: true
        );

        if (result.Succeeded)
        {
            await _authService.UpdateUserLastActivityDateAsync(user.Id);
            return Ok();
        }
        else if (result.IsLockedOut)
        {
            return BadRequest("Account is locked out");
        }
        else
        {
            return BadRequest("Invalid email and/or password");
        }
    }

    [HttpPost("signup")]
    [ValidateModel]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO form)
    {
        var user = new Members
        {
            UserName = form.UserName,
            Email = form.Email,
            FirstName = form.FirstName,
            LastName = form.LastName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createUserResult = await _userManager.CreateAsync(user, form.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest(createUserResult.Errors);
        }

        await _signInManager.SignInAsync(user, true);
        return Ok(new { UserId = user.Id });
    }

    [HttpPost("signout")]
    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userId))
            {
                await _authService.UpdateUserLastActivityDateAsync(Guid.Parse(userId));
            }
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        return BadRequest("User is not logged in");
    }
}
