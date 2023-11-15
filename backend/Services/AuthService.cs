using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class AuthService
{
    private readonly DataContext _context;
    private readonly UserManager<Members> _userManager;

    public AuthService(DataContext context, UserManager<Members> userManager)
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

    public async Task<string> GetEmailConfirmationHtml(string callbackUrl)
    {
        var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "EmailTemplates",
            "EmailConfirmationTemplate.html"
        );
        var htmlContent = await File.ReadAllTextAsync(filePath);
        return htmlContent.Replace("{callbackUrl}", callbackUrl);
    }

    public async Task<bool> CanSendNewConfirmationEmail(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
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
            else
            {
                return true;
            }
        }

        throw new Exception("Cant find user!");
    }
}
