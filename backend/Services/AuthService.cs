using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class AuthService
{
    private readonly DataContext _context;
    private readonly UserManager<Member> _userManager;

    private readonly ILogger<AuthService> _logger;

    public AuthService(
        DataContext context,
        UserManager<Member> userManager,
        ILogger<AuthService> logger
    )
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task UpdateUserLastActivityDateAsync(Guid userId)
    {
        var lastActivityDate = DateTime.UtcNow;
        await _context
            .Database
            .ExecuteSqlAsync(
                $"UPDATE member SET updated_at = {lastActivityDate} WHERE id = {userId} "
            );
    }

    public async Task<string> GetEmailConfirmationHtml(string callbackUrl, string emailTemplate)
    {
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "EmailTemplates",
            emailTemplate
        );
        var htmlContent = await File.ReadAllTextAsync(filePath);
        return htmlContent.Replace("{callbackUrl}", callbackUrl);
    }

    public Task<bool> CanSendNewConfirmationEmail(Member user)
    {
        if (user != null)
        {
            DateTime LastEmailConfirmationSentDate = user.LastEmailConfirmationSentDate;
            DateTime CurrentDateTime = DateTime.UtcNow;
            int emailConfirmationSentCount = user.EmailConfirmationSentCount;

            var twentyFourHoursPassed =
                (CurrentDateTime - LastEmailConfirmationSentDate) > TimeSpan.FromHours(24);

            if (!twentyFourHoursPassed && emailConfirmationSentCount >= 3)
            {
                return Task.FromResult(false);
            }
            else if (twentyFourHoursPassed)
            {
                user.EmailConfirmationSentCount = 0;
                return Task.FromResult(true);
            }
            else
            {
                // 24 hours have not passed but we are under the daily limit
                return Task.FromResult(true);
            }
        }

        throw new Exception("Cant find user!");
    }

    public Task<bool> CanSendNewPasswordResetEmail(Member user)
    {
        if (user != null)
        {
            DateTime LastEmailConfirmationSentDate = user.LastPasswordResetEmailSentDate;
            DateTime CurrentDateTime = DateTime.UtcNow;
            int passwordResetSentCount = user.PasswordResetEmailSentCount;

            var twentyFourHoursPassed =
                (CurrentDateTime - LastEmailConfirmationSentDate) > TimeSpan.FromHours(24);

            if (!twentyFourHoursPassed && passwordResetSentCount >= 3)
            {
                return Task.FromResult(false);
            }
            else if (twentyFourHoursPassed)
            {
                user.PasswordResetEmailSentCount = 0;
                return Task.FromResult(true);
            }
            else
            {
                // 24 hours have not passed but we are under the daily limit
                return Task.FromResult(true);
            }
        }

        throw new Exception("Cant find user!");
    }

    public async Task<string> GenerateUniqueUserName(string firstName, string lastName)
    {
        var baseUserName = $"{firstName}{lastName}";
        var uniqueUserName = baseUserName;

        while (await UsernameExists(uniqueUserName))
        {
            // Generate a random number or string.
            var randomString = Guid.NewGuid().ToString("N").Substring(0, 6); // 6 characters long
            uniqueUserName = $"{baseUserName}{randomString}";
        }

        return uniqueUserName;
    }

    public async Task<bool> UsernameExists(string username)
    {
        var normalizedUserName = username.ToUpperInvariant();
        var usernameExists = await _context
            .Member
            .FromSql($"SELECT * FROM member WHERE normalized_user_name = {normalizedUserName}")
            .FirstOrDefaultAsync();

        if (usernameExists != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<IdentityResult> LinkExternalLogin(
        Member user,
        ExternalLoginInfo externalLoginInfo
    )
    {
        IdentityResult result = await _userManager.AddLoginAsync(user, externalLoginInfo);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to link external login for user {UserId}", user.Id);
            return result;
        }

        var pictureClaim = externalLoginInfo.Principal.FindFirst("urn:google:picture");
        if (pictureClaim != null)
        {
            await _userManager.AddClaimAsync(user, pictureClaim);
        }
        else
        {
            _logger.LogWarning("Google picture claim data is missing for {UserId}", user.Id);
        }

        _logger.LogInformation("Successfully linked external login for {UserId}", user.Id);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> CreateAndLinkExternalLogin(
        Member newUser,
        ExternalLoginInfo externalLoginInfo
    )
    {
        IdentityResult result = await _userManager.CreateAsync(newUser);

        if (!result.Succeeded)
        {
            _logger.LogError(
                "Failed to created account and link external login for user {UserId}",
                newUser.Id
            );
            return result;
        }

        result = await _userManager.AddLoginAsync(newUser, externalLoginInfo);
        if (result.Succeeded)
        {
            var pictureClaim = externalLoginInfo.Principal.FindFirst("urn:google:picture");
            if (pictureClaim != null)
            {
                await _userManager.AddClaimAsync(newUser, pictureClaim);
                return result;
            }
            else
            {
                _logger.LogWarning("Google picture claim data is missing for {UserId}", newUser.Id);
            }
        }
        _logger.LogInformation(
            "Successfully created account and linked external login for {UserId}",
            newUser.Id
        );
        return result;
    }
}
