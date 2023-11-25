using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class AuthService
{
    private readonly DataContext _context;
    private readonly UserManager<Member> _userManager;

    private readonly SignInManager<Member> _signInManager;

    public AuthService(DataContext context, UserManager<Member> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task UpdateUserLastActivityDateAsync(Guid userId)
    {
        var lastActivityDate = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
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

    public async Task<bool> CanSendNewConfirmationEmail(Member user)
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
                return false;
            }
            else if (twentyFourHoursPassed)
            {
                user.EmailConfirmationSentCount = 0;
                return true;
            }
            else
            {
                // 24 hours have not passed but we are under the daily limit
                return true;
            }
        }

        throw new Exception("Cant find user!");
    }

    public async Task<bool> CanSendNewPasswordResetEmail(Member user)
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
                return false;
            }
            else if (twentyFourHoursPassed)
            {
                user.PasswordResetEmailSentCount = 0;
                return true;
            }
            else
            {
                // 24 hours have not passed but we are under the daily limit
                return true;
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
        var usernameExists = await _context.Member
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
        if (result.Succeeded)
        {
            if (externalLoginInfo.Principal.HasClaim(c => c.Type == "urn:google:picture"))
            {
                await _userManager.AddClaimAsync(
                    user,
                    externalLoginInfo.Principal.FindFirst("urn:google:picture")
                );
            }

            await _signInManager.SignInAsync(user, isPersistent: true);
            return result;
        }
        else
        {
            return result;
        }
    }

    public async Task<IdentityResult> CreateAndLinkExternalLogin(
        Member newUser,
        ExternalLoginInfo externalLoginInfo
    )
    {
        IdentityResult result = await _userManager.CreateAsync(newUser);
        if (result.Succeeded)
        {
            result = await _userManager.AddLoginAsync(newUser, externalLoginInfo);
            if (result.Succeeded)
            {
                if (externalLoginInfo.Principal.HasClaim(c => c.Type == "urn:google:picture"))
                {
                    await _userManager.AddClaimAsync(
                        newUser,
                        externalLoginInfo.Principal.FindFirst("urn:google:picture")
                    );
                }
                return result;
            }
            return result;
        }
        else
        {
            return result;
        }
    }
}