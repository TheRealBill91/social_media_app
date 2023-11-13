using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Services;

public class AuthService
{
    private readonly DataContext _context;
    private readonly MemberService _memberService;

    public AuthService(DataContext context, MemberService memberService)
    {
        _context = context;
        _memberService = memberService;
    }

    public async Task UpdateUserLastActivityDateAsync(Guid userId)
    {
        var lastActivityDate = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE members SET updated_at = {lastActivityDate} WHERE id = {userId} "
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

    public async Task<bool> CanSendNewConfirmationEmail(Guid userId)
    {
        var user = await _memberService.GetMemberAsync(userId);
        if (user != null)
        {
            DateTime LastEmailConfirmationSentDate = user.LastEmailConfirmationSentDate;
            DateTime CurrentDateTime = DateTime.UtcNow;
            int emailConfirmationSentCount = user.EmailConfirmationSentCount;

            var twentyFourHoursPassed =
                (CurrentDateTime - LastEmailConfirmationSentDate) > TimeSpan.FromHours(24);

            if (!twentyFourHoursPassed && emailConfirmationSentCount >= 2)
            {
                return false;
            }
            else if (twentyFourHoursPassed)
            {
                return true;
            }
        }

        throw new Exception("Cant find user!");
    }
}
