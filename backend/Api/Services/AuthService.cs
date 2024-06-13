using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class AuthService
{
    private readonly DataContext _context;
    private readonly UserManager<Member> _userManager;

    private readonly MemberService _memberService;

    private readonly ILogger<AuthService> _logger;

    public AuthService(
        DataContext context,
        UserManager<Member> userManager,
        MemberService memberService,
        ILogger<AuthService> logger
    )
    {
        _context = context;
        _userManager = userManager;
        _memberService = memberService;
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

    public async Task<string> GenerateEmailContentFromTemplate(
        string callbackUrl,
        string emailTemplate
    )
    {
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "EmailTemplates",
            emailTemplate
        );
        var htmlContent = await File.ReadAllTextAsync(filePath);
        return htmlContent.Replace("{callbackUrl}", callbackUrl);
    }

    public async Task<string> GenerateForgotUsernameEmailHtml(string username, string emailTemplate)
    {
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "EmailTemplates",
            emailTemplate
        );

        var htmlContent = await File.ReadAllTextAsync(filePath);
        return htmlContent.Replace("{username}", username);
    }

    public bool CanSendNewConfirmationEmail(Member user)
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

    public bool CanSendNewPasswordResetEmail(Member user)
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

    public bool CanSendUsernameEmail(Member user)
    {
        if (user != null)
        {
            var LastUserNameRequestEmailSentDate = user.LastUsernameRequestEmailSentDate;
            var CurrentDateTime = DateTime.UtcNow;
            int usernameRequestEmailSentCount = user.UsernameRequestEmailSentCount;

            var twentyFourHoursPassed =
                (CurrentDateTime - LastUserNameRequestEmailSentDate) > TimeSpan.FromHours(24);

            if (!twentyFourHoursPassed && usernameRequestEmailSentCount >= 3)
            {
                return false;
            }
            else if (twentyFourHoursPassed)
            {
                user.UsernameRequestEmailSentCount = 0;
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

        while (await _memberService.UsernameExists(uniqueUserName))
        {
            // Generate a random number or string.
            var randomString = Guid.NewGuid().ToString("N").Substring(0, 6); // 6 characters long
            uniqueUserName = $"{baseUserName}{randomString}";
        }

        return uniqueUserName;
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

        var pictureClaim = externalLoginInfo.Principal.FindFirst("picture");
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

    public async Task<AddPasswordToHistoryResponse> AddPasswordToHistory(
        string passwordHash,
        Guid userId
    )
    {
        var result = await _context
            .Database
            .ExecuteSqlAsync(
                $"INSERT INTO password_history (id, password_hash) VALUES ( {userId}, {passwordHash})"
            );

        if (result > 0)
        {
            return new AddPasswordToHistoryResponse { Success = true };
        }
        else
        {
            return new AddPasswordToHistoryResponse
            {
                Success = false,
                Message = "Password has previously been used, please try again"
            };
        }
    }

    public async Task<PasswordExistsResponse> PasswordAlreadyExists(Member user, string newPassword)
    {
        var passwordHashes = await _context
            .PasswordHistory
            .Select(p => p.PasswordHash)
            .Distinct()
            .ToListAsync();

        var hasUsedPassword = passwordHashes.Any(hash =>
        {
            var res = _userManager.PasswordHasher.VerifyHashedPassword(user, hash, newPassword);
            return res == PasswordVerificationResult.Success;
        });

        if (hasUsedPassword)
        {
            return new PasswordExistsResponse
            {
                Result = true,
                Message = "Password previously used, please try a different one"
            };
        }
        else
        {
            return new PasswordExistsResponse { Result = false };
        }
    }
}
