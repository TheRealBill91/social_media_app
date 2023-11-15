using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SendGrid.Helpers.Errors.Model;
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

    private readonly IEmailSender _emailSender;

    private readonly MemberService _memberService;

    private readonly IConfiguration _configuration;

    public AuthController(
        SignInManager<Members> signInManager,
        UserManager<Members> userManager,
        AuthService authService,
        IEmailSender emailSender,
        MemberService memberService,
        IConfiguration configuration
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authService = authService;
        _emailSender = emailSender;
        _memberService = memberService;
        _configuration = configuration;
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

        bool rememberMe = form.RememberMe == true;

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            form.Password,
            isPersistent: rememberMe, // For 'Remember me' functionality
            lockoutOnFailure: true
        );

        if (result.Succeeded)
        {
            await _authService.UpdateUserLastActivityDateAsync(user.Id);
            return Ok();
        }
        else if (result.IsLockedOut)
        {
            return BadRequest("Too many failed login attempts, please try in 30 minutes");
        }
        else if (result.IsNotAllowed)
        {
            return BadRequest("Please confirm your email to sign in");
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
            UpdatedAt = DateTime.UtcNow,
            LastEmailConfirmationSentDate = DateTime.UtcNow,
            EmailConfirmationSentCount = 1
        };

        var createUserResult = await _userManager.CreateAsync(user, form.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest(createUserResult.Errors);
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var baseUrl = _configuration["ApiSettings:BaseUrl"];

        var callbackURL =
            $"{baseUrl}/auth/confirmemail?userId={user.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

        string emailHTML = await _authService.GetEmailConfirmationHtml(callbackURL);

        await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailHTML);

        return Ok(new { UserId = user.Id });
    }

    [HttpGet("confirmemail")]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound("User not found");

        var decodedCode = WebEncoders.Base64UrlDecode(code);
        var result = await _userManager.ConfirmEmailAsync(
            user,
            Encoding.UTF8.GetString(decodedCode)
        );

        if (result.Succeeded)
        {
            await _authService.UpdateUserLastActivityDateAsync(user.Id);
            return Ok("Email confirmed successfully");
        }
        else
        {
            bool tokenExpired = result.Errors
                .Select(e => e.Description)
                .Any(e => e.Contains("invalid token.", StringComparison.OrdinalIgnoreCase));

            if (tokenExpired)
                return BadRequest("Password reset has expired");
            else
            {
                return BadRequest("Error confirming email confirmation");
            }
        }
    }

    [HttpPost("signout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
            await _authService.UpdateUserLastActivityDateAsync(Guid.Parse(userId));
        }
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpPost("resendemailconfirmation")]
    [ValidateModel]
    public async Task<IActionResult> ResendEmailConfirmation(
        [FromBody] ResendEmailConfirmationDTO form
    )
    {
        var userEmail = form.Email;
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user != null)
        {
            var userId = user.Id.ToString();
            bool canRequestEmailConfirmation = await _authService.CanSendNewConfirmationEmail(
                userId
            );

            if (canRequestEmailConfirmation && user != null)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var baseUrl = _configuration["ApiSettings:BaseUrl"];

                var callbackURL =
                    $"{baseUrl}/auth/confirmemail?userId={user.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

                string emailHTML = await _authService.GetEmailConfirmationHtml(callbackURL);

                await _emailSender.SendEmailAsync(user.Email!, "Confirm your email", emailHTML);

                await _memberService.UpdateEmailConfirmationSendDate(Guid.Parse(userId));

                await _memberService.UpdateEmailConfirmationSendCount(
                    user.EmailConfirmationSentCount,
                    Guid.Parse(userId)
                );

                return Ok();
            }
            else
            {
                return StatusCode(429, "Too many email confirmations sent today, try tomorrow");
            }
        }

        return NotFound();
    }

    [HttpGet("google-login")]
    [AllowAnonymous]
    public async Task<IActionResult> GoogleLogin(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/"); // Fallback to home page if no returnUrl is specified
        var properties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("GoogleResponse", "Auth"),
            Items = { { "returnUrl", returnUrl } }
        };

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

        var claims = result.Principal.Identities
            .FirstOrDefault()
            .Claims.Select(
                claim =>
                    new
                    {
                        claim.Issuer,
                        claim.OriginalIssuer,
                        claim.Type,
                        claim.Value
                    }
            );

        return Ok();
    }
}
