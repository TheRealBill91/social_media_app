using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<Member> _signInManager;

    private readonly UserManager<Member> _userManager;

    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    private readonly AuthService _authService;

    private readonly IEmailSender _emailSender;

    private readonly IConfiguration _configuration;

    private readonly MemberService _memberService;

    private readonly MemberProfileService _memberProfileService;

    public AuthController(
        SignInManager<Member> signInManager,
        UserManager<Member> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        AuthService authService,
        IEmailSender emailSender,
        IConfiguration configuration,
        MemberService memberService,
        MemberProfileService memberProfileService
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
        _emailSender = emailSender;
        _configuration = configuration;
        _memberService = memberService;
        _memberProfileService = memberProfileService;
    }

    [EnableRateLimiting("signInSlidingWindow")]
    [HttpPost("signin")]
    [ValidateModel]
    public async Task<IActionResult> Signin([FromBody] SignInDTO form)
    {
        if (User.Identity!.IsAuthenticated)
        {
            return BadRequest("User is already authenticated");
        }

        // Find the user by email
        Member? user;

        user = await _userManager.FindByEmailAsync(form.EmailOrUsername);

        // checks if user is null before finding by username
        user ??= await _userManager.FindByNameAsync(form.EmailOrUsername);

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
            return BadRequest("Too many failed signin attempts, please try in 30 minutes");
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

    [EnableRateLimiting("signUpSlidingWindow")]
    [HttpPost("signup")]
    [ValidateModel]
    public async Task<IActionResult> SignUp([FromBody] SignUpDTO form)
    {
        var newUser = new Member
        {
            UserName = form.UserName,
            Email = form.Email,
            FirstName = form.FirstName,
            LastName = form.LastName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            LastEmailConfirmationSentDate = DateTime.UtcNow,
            EmailConfirmationSentCount = 1,
            PasswordResetEmailSentCount = 0
        };

        var user = await _userManager.FindByEmailAsync(form.Email);

        var passwordValidator = new PasswordValidator<Member>();
        var userValidator = new UserValidator<Member>();

        var passwordResult = await passwordValidator.ValidateAsync(
            _userManager,
            newUser,
            form.Password
        );
        var userResult = await userValidator.ValidateAsync(_userManager, newUser);

        if (user != null)
        {
            var logins = await _userManager.GetLoginsAsync(user);
            var googleLoginExists = logins.FirstOrDefault(
                l => l.LoginProvider == GoogleDefaults.AuthenticationScheme
            );

            bool passwordSet = await _userManager.HasPasswordAsync(user);

            // google external account exists but no password set on it (yet)
            if (googleLoginExists != null && !passwordSet)
            {
                var addPasswordResult = await _userManager.AddPasswordAsync(user, form.Password);
                if (addPasswordResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return Ok();
                }
                else
                {
                    return BadRequest(addPasswordResult.Errors);
                }
            }
            else
            {
                // User with local account (no google linked login) already exists
                if (!passwordResult.Succeeded || !userResult.Succeeded)
                {
                    if (!passwordResult.Succeeded)
                    {
                        foreach (var error in passwordResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                    if (!userResult.Succeeded)
                    {
                        foreach (var error in userResult.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }
                }

                return BadRequest(ModelState);
            }
        }

        if (!passwordResult.Succeeded || !userResult.Succeeded)
        {
            var errors = passwordResult.Errors.Concat(userResult.Errors);
            foreach (var error in errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return BadRequest(ModelState);
        }

        var createUserResult = await _userManager.CreateAsync(newUser, form.Password);
        if (!createUserResult.Succeeded)
        {
            return BadRequest(createUserResult.Errors);
        }

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

        var baseUrl = _configuration["ApiSettings:BaseUrl"];
        var frontendURL = _configuration["ApiSettings:FrontendUrl"];

        var callbackURL =
            $"{frontendURL}/confirm-email?userId={newUser.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

        string emailHTML = await _authService.GetEmailConfirmationHtml(
            callbackURL,
            "EmailConfirmationTemplate.html"
        );

        await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email", emailHTML);

        return Ok(new { UserId = newUser.Id });
    }

    [EnableRateLimiting("confirmEmailSlidingWindow")]
    [HttpGet("confirmemail")]
    public async Task<IActionResult> ConfirmEmail(Guid userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return NotFound("User not found");
        }

        var decodedCode = WebEncoders.Base64UrlDecode(code);
        var result = await _userManager.ConfirmEmailAsync(
            user,
            Encoding.UTF8.GetString(decodedCode)
        );

        if (result.Succeeded)
        {
            // create member profile
            var memberProfileCreation = await _memberProfileService.CreateMemberProfile(userId);
            if (!memberProfileCreation.Success)
            {
                // should not get here
                return BadRequest("failed to create the member profile");
            }

            await _authService.UpdateUserLastActivityDateAsync(user.Id);
            return Ok("Email confirmed successfully");
        }
        else
        {
            bool tokenExpired = result
                .Errors
                .Select(e => e.Description)
                .Any(e => e.Contains("invalid token.", StringComparison.OrdinalIgnoreCase));

            if (tokenExpired)
            {
                var errorResponse = new EmailConfirmationResponse
                {
                    Error = "Email Confirmation has expired",
                    Email = user.Email!
                };
                return BadRequest(errorResponse);
            }
            else
            {
                return BadRequest("Error confirming email confirmation");
            }
        }
    }

    [EnableRateLimiting("signoutSlidingWindow")]
    [HttpPost("signout")]
    [Authorize]
    public async Task<IActionResult> Signout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrEmpty(userId))
        {
            await _authService.UpdateUserLastActivityDateAsync(Guid.Parse(userId));
        }
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    [EnableRateLimiting("resendEmailConfirmationSlidingWindow")]
    [HttpPost("resend-email-confirmation")]
    [ValidateModel]
    public async Task<IActionResult> ResendEmailConfirmation(
        [FromBody] ResendEmailConfirmationDTO form
    )
    {
        string userEmail = form.Email;
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user != null)
        {
            var emailIsVerified = await _userManager.IsEmailConfirmedAsync(user);
            if (emailIsVerified)
            {
                // will only be reached if user is allowed to manually input email to send new confirmation email
                return new JsonResult(
                    new { error = "Email is already verified. No new confirmation link sent." }
                )
                {
                    StatusCode = 409
                };
            }
            string userId = user.Id.ToString();
            bool canRequestEmailConfirmation = await _authService.CanSendNewConfirmationEmail(user);

            if (canRequestEmailConfirmation)
            {
                string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                string baseUrl = _configuration["ApiSettings:BaseUrl"]!;

                string callbackURL =
                    $"{baseUrl}/auth/confirmemail?userId={user.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

                string emailHTML = await _authService.GetEmailConfirmationHtml(
                    callbackURL,
                    "EmailConfirmationTemplate.html"
                );

                await _emailSender.SendEmailAsync(user.Email!, "Confirm your email", emailHTML);

                await _memberService.UpdateEmailConfirmationSendDate(Guid.Parse(userId));

                await _memberService.UpdateEmailConfirmationSendCount(
                    user.EmailConfirmationSentCount,
                    Guid.Parse(userId)
                );

                return Ok("Successfully resent email confirmation");
            }
            else
            {
                return new JsonResult(
                    new { error = "Too many email confirmations sent today, try in 24 hours" }
                )
                {
                    StatusCode = 429
                };
            }
        }

        return NotFound("Issue sending email confirmation");
    }

    [EnableRateLimiting("signInSlidingWindow")]
    [HttpGet("google-sign-in")]
    [AllowAnonymous]
    public IActionResult GoogleSignin(string? returnUrl = null)
    {
        var redirectUrl = Url.Action("GoogleResponse", "Auth");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(
            GoogleDefaults.AuthenticationScheme,
            redirectUrl
        );

        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [EnableRateLimiting("signInSlidingWindow")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GoogleResponse()
    {
        var externalSigninInfo = await _signInManager.GetExternalLoginInfoAsync();

        if (externalSigninInfo == null)
        {
            return BadRequest();
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            externalSigninInfo.LoginProvider,
            externalSigninInfo.ProviderKey,
            isPersistent: true
        );

        var claims = externalSigninInfo.Principal.Claims;

        Claim? emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        Claim? firstNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName);
        Claim? lastNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname);
        string userName = await _authService.GenerateUniqueUserName(
            firstNameClaim!.Value,
            lastNameClaim!.Value
        );

        if (signInResult.Succeeded)
        {
            // return Redirect("http://localhost:5151/swagger/index.html");

            return Ok();
        }
        else if (signInResult.IsLockedOut)
        {
            return BadRequest("Too many failed login attempts, please try in 30 minutes");
        }
        // local account with same email does not exist, sign user up using google account
        else
        {
            if (emailClaim == null)
            {
                return BadRequest("email claim is missing");
            }

            var user = await _userManager.FindByEmailAsync(emailClaim.Value);

            if (user != null)
            {
                IdentityResult externalLinkResult = await _authService.LinkExternalLogin(
                    user,
                    externalSigninInfo
                );
                if (externalLinkResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return Ok();
                }
                else
                {
                    return BadRequest(externalLinkResult.Errors);
                }
            }
            else
            {
                var newUser = new Member
                {
                    UserName = userName,
                    Email = emailClaim.Value,
                    FirstName = firstNameClaim.Value,
                    LastName = lastNameClaim.Value,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    EmailConfirmationSentCount = 0
                };
                var externalCreatationLoginResult = await _authService.CreateAndLinkExternalLogin(
                    newUser,
                    externalSigninInfo
                );
                if (externalCreatationLoginResult.Succeeded)
                {
                    // create member profile
                    var memberProfileCreation = await _memberProfileService.CreateMemberProfile(
                        newUser.Id
                    );
                    if (!memberProfileCreation.Success)
                    {
                        // should not get here
                        return BadRequest("failed to create the member profile");
                    }
                    await _signInManager.SignInAsync(newUser, isPersistent: true);
                    return Ok();
                }
                else
                {
                    return BadRequest(externalCreatationLoginResult.Errors);
                }
            }
        }
    }

    [EnableRateLimiting("passwordResetRequestSlidingWindow")]
    [HttpPost("passwordresetrequest")]
    [ValidateModel]
    public async Task<IActionResult> RequestPasswordResetEmail(
        [FromBody] RequestPasswordResetDTO form
    )
    {
        string userEmail = form.Email;
        var user = await _userManager.FindByEmailAsync(userEmail);

        if (user != null)
        {
            var userId = user.Id.ToString();
            bool canRequestPasswordResetEmail = await _authService.CanSendNewPasswordResetEmail(
                user
            );

            if (canRequestPasswordResetEmail)
            {
                user.PasswordResetEmailSentCount = 0;
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);

                string? baseUrl = _configuration["ApiSettings:BaseUrl"];

                string callbackURL =
                    $"{baseUrl}/auth/validate-password-reset-token?userId={user.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

                string emailHTML = await _authService.GetEmailConfirmationHtml(
                    callbackURL,
                    "EmailPasswordResetTemplate.html"
                );

                await _emailSender.SendEmailAsync(user.Email!, "Reset your password", emailHTML);

                await _memberService.UpdatePasswordResetSendDate(Guid.Parse(userId));

                await _memberService.UpdatePasswordResetSendCount(
                    user.PasswordResetEmailSentCount,
                    Guid.Parse(userId)
                );

                return Ok();
            }
            else
            {
                return StatusCode(429, "Too many password reset emails sent today, try tomorrow");
            }
        }
        return NotFound("Issue sending email confirmation");
    }

    [EnableRateLimiting("passwordResetRequestSlidingWindow")]
    [HttpGet("validate-password-reset-token")]
    public async Task<IActionResult> ValidatePasswordResetToken(Guid userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return NotFound("User not found");
        }

        var decodedCode = WebEncoders.Base64UrlDecode(code);
        var passwordResetTokenValid = await _userManager.VerifyUserTokenAsync(
            user,
            "Default",
            "ResetPassword",
            Encoding.UTF8.GetString(decodedCode)
        );

        if (passwordResetTokenValid)
        {
            return Ok("Password reset token is valid");
        }
        else if (!passwordResetTokenValid)
        {
            return BadRequest("Invalid password reset token.");
        }
        else
        {
            throw new Exception("Some other issue is going on...");
        }
    }

    [EnableRateLimiting("passwordResetRequestSlidingWindow")]
    [HttpPost("resetpassword")]
    [ValidateModel]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordDTO form,
        Guid userId,
        string code
    )
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user != null)
        {
            var decodedCode = WebEncoders.Base64UrlDecode(code);
            var passwordResetResult = await _userManager.ResetPasswordAsync(
                user,
                Encoding.UTF8.GetString(decodedCode),
                form.NewPassword
            );

            if (passwordResetResult.Succeeded)
            {
                await _authService.UpdateUserLastActivityDateAsync(user.Id);
                return Ok("Password reset successful");
            }
            else
            {
                return BadRequest(passwordResetResult.Errors);
            }
        }
        else
        {
            return BadRequest("User was not found");
        }
    }

    /*  [HttpPost("admin-signup")]
     [ValidateModel]
     public async Task<IActionResult> AdminSignUp([FromBody] SignUpDTO form)
     {
         // Check if admin role exists, create if not
         if (!await _roleManager.RoleExistsAsync("Admin"))
         {
             await _roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
         }
 
         var adminUser = new Member
         {
             UserName = form.UserName,
             Email = form.Email,
             FirstName = form.FirstName,
             LastName = form.LastName,
             CreatedAt = DateTime.UtcNow,
             UpdatedAt = DateTime.UtcNow,
             LastEmailConfirmationSentDate = DateTime.UtcNow,
             EmailConfirmationSentCount = 1,
             PasswordResetEmailSentCount = 0
         };
 
         var result = await _userManager.CreateAsync(adminUser, form.Password);
 
         if (!result.Succeeded)
         {
             return BadRequest(result.Errors);
         }
 
         await _userManager.AddToRoleAsync(adminUser, "Admin");
 
         var code = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
 
         var baseUrl = _configuration["ApiSettings:BaseUrl"];
 
         var callbackURL =
             $"{baseUrl}/auth/confirmemail?userId={adminUser.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";
 
         string emailHTML = await _authService.GetEmailConfirmationHtml(
             callbackURL,
             "EmailConfirmationTemplate.html"
         );
 
         await _emailSender.SendEmailAsync(adminUser.Email, "Confirm your email", emailHTML);
 
         return Ok(new { UserId = adminUser.Id });
     } */
}
