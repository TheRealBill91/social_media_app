using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using SocialMediaApp.Filters;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<Member> _signInManager;

    private readonly UserManager<Member> _userManager;

    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    private readonly AuthService _authService;

    private readonly IEmailSender _emailSender;

    private readonly ApiSettingsOptions _apiSettingsOptions;

    private readonly MemberService _memberService;

    private readonly MemberProfileService _memberProfileService;

    public AuthController(
        SignInManager<Member> signInManager,
        UserManager<Member> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        AuthService authService,
        IEmailSender emailSender,
        IOptionsSnapshot<ApiSettingsOptions> apiSettingsOptions,
        MemberService memberService,
        MemberProfileService memberProfileService
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _authService = authService;
        _emailSender = emailSender;
        _apiSettingsOptions = apiSettingsOptions.Value;
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
            return BadRequest(new { ErrorMessage = "User is already authenticated" });
        }

        // Find the user by email


        var user = await _userManager.FindByEmailAsync(form.EmailOrUsername);

        // checks if user is null before finding by username
        user ??= await _userManager.FindByNameAsync(form.EmailOrUsername);

        if (user == null || user.UserName == null)
        {
            return BadRequest(new { ErrorMessage = "Invalid email and/or password" });
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
            return Ok(new { UserId = user.Id });
        }
        else if (result.IsLockedOut)
        {
            return BadRequest(
                new { ErrorMessage = "Too many failed signin attempts, please try in 30 minutes" }
            );
        }
        else if (result.IsNotAllowed)
        {
            return BadRequest(new { ErrorMessage = "Please confirm your email to sign in" });
        }
        else
        {
            return BadRequest(new { ErrorMessage = "Invalid email and/or password" });
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
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        var createUserResult = await _userManager.CreateAsync(newUser, form.Password);

        var newLocalUser = await _userManager.FindByEmailAsync(form.Email);

        if (!createUserResult.Succeeded)
        {
            return BadRequest(createUserResult.Errors);
        }

        if (newLocalUser == null)
        {
            // should not get here
            return StatusCode(500);
        }

        var passwordHash = newLocalUser.PasswordHash;

        var userId = newLocalUser.Id;

        var addPasswordToHistoryResult = await _authService.AddPasswordToHistory(
            passwordHash!,
            userId
        );

        if (!addPasswordToHistoryResult.Succeess)
        {
            // should not get here
            return StatusCode(500);
        }

        // * Commented out just for testing AddPasswordToHistory functionality

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

        var baseUrl = _apiSettingsOptions.BaseUrl;
        var frontendURL = _apiSettingsOptions.FrontendUrl;

        var callbackURL =
            $"{frontendURL}/confirm-email?userId={newUser.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

        string emailHTML = await _authService.GetEmailConfirmationHtml(
            callbackURL,
            "EmailConfirmationTemplate.html"
        );

        await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email", emailHTML);

        return Ok(new { UserId = newUser.Id });
    }

    //[EnableRateLimiting("confirmEmailSlidingWindow")]
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDTO emailFields)
    {
        var user = await _userManager.FindByIdAsync(emailFields.UserId.ToString());
        if (user == null)
        {
            return NotFound(new { ErrorMessage = "User not found" });
        }

        var decodedCode = WebEncoders.Base64UrlDecode(emailFields.Code);
        var result = await _userManager.ConfirmEmailAsync(
            user,
            Encoding.UTF8.GetString(decodedCode)
        );

        if (result.Succeeded)
        {
            // create member profile
            var memberProfileCreation = await _memberProfileService.CreateMemberProfile(
                emailFields.UserId
            );
            if (!memberProfileCreation.Success)
            {
                // should not get here
                return BadRequest(new { ErrorMessage = "Failed to create the member profile." });
            }

            await _authService.UpdateUserLastActivityDateAsync(user.Id);
            return Ok();
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
                    ErrorMessage = "Email Confirmation has expired",
                    Email = user.Email!
                };
                return BadRequest(errorResponse);
            }
            else
            {
                return BadRequest(new { ErrorMessage = "Error confirming email confirmation" });
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

    [EnableRateLimiting("resendConfirmationEmailSlidingWindow")]
    [HttpPost("resend-confirmation-email")]
    [ValidateModel]
    public async Task<IActionResult> ResendConfirmationEmail(
        [FromBody] ResendConfirmationEmailDTO form
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
                    new
                    {
                        ErrorMessage = "Email is already verified. No new confirmation link sent."
                    }
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

                string baseUrl = _apiSettingsOptions.BaseUrl;

                string callbackURL =
                    $"{baseUrl}/auth/confirmemail?userId={user.Id}&code={WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code))}";

                string emailHTML = await _authService.GetEmailConfirmationHtml(
                    callbackURL,
                    "EmailConfirmationTemplate.html"
                );

                if (user.Email == null)
                {
                    return BadRequest(new { ErrorMessage = "Email is missing" });
                }

                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", emailHTML);

                await _memberService.UpdateEmailConfirmationSendDate(Guid.Parse(userId));

                await _memberService.UpdateEmailConfirmationSendCount(
                    user.EmailConfirmationSentCount,
                    Guid.Parse(userId)
                );

                return Ok(new { successMessage = "Successfully resent email confirmation" });
            }
            else
            {
                Console.WriteLine("getting here?");
                return new JsonResult(
                    new
                    {
                        ErrorMessage = "Too many email confirmations sent today, try in 24 hours"
                    }
                )
                {
                    StatusCode = 429
                };
            }
        }

        return NotFound(new { ErrorMessage = "Issue sending email confirmation" });
    }

    // [EnableRateLimiting("signInSlidingWindow")]
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

    //[EnableRateLimiting("signInSlidingWindow")]
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GoogleResponse()
    {
        var frontendURL = _apiSettingsOptions.FrontendUrl;

        // cookie options for the cookie we return in the redirect to the
        // Remix BFF
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromMinutes(3),
            Path = "/auth/google/callback",
        };

        // return URL of the Remix BFF callback loader
        var redirectURL = $"{frontendURL}/auth/google/callback";

        var externalSigninInfo = await _signInManager.GetExternalLoginInfoAsync();

        if (externalSigninInfo == null)
        {
            Response
                .Cookies
                .Append("ExternalSigninError", "We ran into an unexpected issue", cookieOptions);
            return Redirect(redirectURL);
        }

        await _signInManager.SignOutAsync();

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
        // User successfully signed in with Google; no further action needed.
        {
            var user = await _userManager.FindByEmailAsync(emailClaim.Value);
            var userId = user.Id.ToString();

            // return userId so Remix can easily query logged in users data

            // override default maxAge to match the auth cookie maxAge
            cookieOptions.MaxAge = TimeSpan.FromDays(3);

            Response
                .Cookies
                .Append("MessageCookie", "Sign in successful! Welcome back.", cookieOptions);

            // override the path so the user id cookie persists across different
            // paths on the frontend
            cookieOptions.Path = "/";

            Response.Cookies.Append("UserId", userId, cookieOptions);

            return Redirect(redirectURL);
        }
        else if (signInResult.IsLockedOut)
        {
            Response
                .Cookies
                .Append(
                    "LockedOutMessage",
                    "Too many login attempts, try again in 30 minutes",
                    cookieOptions
                );

            return Redirect(redirectURL);
        }
        else
        {
            // Checking if account already exists using email claim
            if (emailClaim == null)
            {
                Response.Cookies.Append("EmailClaimError", "We ran into an unexpected issue");
                return Redirect(redirectURL);
            }

            var user = await _userManager.FindByEmailAsync(emailClaim.Value);

            if (user != null)
            // Linking user's local account to their google account and signing them in
            {
                IdentityResult externalLinkResult = await _authService.LinkExternalLogin(
                    user,
                    externalSigninInfo
                );
                if (externalLinkResult.Succeeded)
                {
                    var userId = user.Id.ToString();

                    // return userId so Remix can easily query logged in users data

                    // override default maxAge to match the auth cookie maxAge
                    cookieOptions.MaxAge = TimeSpan.FromDays(3);

                    Response.Cookies.Append("UserId", userId, cookieOptions);

                    await _signInManager.SignInAsync(user, isPersistent: true);

                    Response
                        .Cookies
                        .Append(
                            "MessageCookie",
                            "Account linked successfully! You are now logged in.",
                            cookieOptions
                        );

                    return Redirect(redirectURL);
                }
                else
                {
                    Response
                        .Cookies
                        .Append(
                            "CreateOrLinkError",
                            "We ran into an issue creating or linking your accounts"
                        );
                    return Redirect(redirectURL);
                }
            }
            else
            // local account with the same email does not exist, creating new account,
            // linking it to the google login, and signing the user in
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
                var externalLoginCreationResult = await _authService.CreateAndLinkExternalLogin(
                    newUser,
                    externalSigninInfo
                );
                if (externalLoginCreationResult.Succeeded)
                {
                    // create member profile
                    var memberProfileCreation = await _memberProfileService.CreateMemberProfile(
                        newUser.Id
                    );
                    if (!memberProfileCreation.Success)
                    {
                        // should not get here
                        Response
                            .Cookies
                            .Append(
                                "CreateOrLinkError",
                                "We ran into an issue creating or linking your accounts"
                            );
                        return Redirect(redirectURL);
                    }

                    var userId = user.Id.ToString();

                    // return userId so Remix can easily query logged in users data

                    // override default maxAge to match the auth cookie maxAge
                    cookieOptions.MaxAge = TimeSpan.FromDays(3);

                    Response.Cookies.Append("UserId", userId, cookieOptions);

                    await _signInManager.SignInAsync(newUser, isPersistent: true);

                    Response
                        .Cookies
                        .Append(
                            "MessageCookie",
                            "Account created successfully! You are now logged in"
                        );

                    return Redirect(redirectURL);
                }
                else
                {
                    Response
                        .Cookies
                        .Append(
                            "CreateOrLinkError",
                            "We ran into an issue creating or linking your accounts"
                        );
                    return Redirect(redirectURL);
                }
            }
        }
    }

    [EnableRateLimiting("passwordResetRequestSlidingWindow")]
    [HttpPost("password-reset-request")]
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

                // just for testing, delete or comment out in prod
                // return Ok(new { code, userId });

                string baseUrl = _apiSettingsOptions.BaseUrl;

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

                return Ok(
                    new
                    {
                        ResponseMessage = "Check your email for instructions on resetting your password"
                    }
                );
            }
            else
            {
                return new JsonResult(
                    new
                    {
                        DailyLimitMessage = "Maximum password reset attempts reached. Please try again tomorrow."
                    }
                )
                {
                    StatusCode = 429
                };
            }
        }
        return NotFound(new { NoAccountFoundMessage = "No account found" });
    }

    [EnableRateLimiting("passwordResetRequestSlidingWindow")]
    [HttpGet("validate-password-reset-token")]
    public async Task<IActionResult> ValidatePasswordResetToken(Guid userId, string code)
    {
        var frontendURL = _apiSettingsOptions.FrontendUrl;
        var redirectURL = $"{frontendURL}/auth/reset-password";

        // cookie options for the cookie we return in the redirect to the
        // Remix BFF
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromMinutes(3),
            Path = "/auth/reset-password",
        };

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            // Should not reach here
            return NotFound(new { ErrorMessage = "User not found" });
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
            Response.Cookies.Append("PasswordResetUserId", user.Id.ToString(), cookieOptions);
            Response.Cookies.Append("Code", code, cookieOptions);
            return Redirect(redirectURL);
        }
        else if (!passwordResetTokenValid)
        {
            Response
                .Cookies
                .Append("ExpiredMessage", "Password reset token is no longer valid", cookieOptions);

            return Redirect(redirectURL);
        }
        else
        {
            throw new Exception("Some other issue is going on...");
        }
    }

    // [EnableRateLimiting("passwordResetRequestSlidingWindow")]
    [HttpPost("reset-password")]
    [ValidateModel]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO form)
    {
        var userId = form.PasswordResetUserId.ToString();
        var code = form.Code;

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            var passwordExists = await _authService.PasswordAlreadyExists(user, form.NewPassword);

            if (passwordExists.Result == true)
            {
                return Conflict(new { passwordExists.Message });
            }
            var decodedCode = WebEncoders.Base64UrlDecode(code);
            var passwordResetResult = await _userManager.ResetPasswordAsync(
                user,
                Encoding.UTF8.GetString(decodedCode),
                form.NewPassword
            );

            if (passwordResetResult.Succeeded)
            {
                await _authService.UpdateUserLastActivityDateAsync(user.Id);
                return Ok(new { SuccessMessage = "Password reset successful" });
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
 
         var baseUrl = _apiSettingsOptions.BaseUrl;
 
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
