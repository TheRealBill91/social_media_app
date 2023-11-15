using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Services;

public class MemberService
{
    private readonly DataContext _context;

    public MemberService(DataContext context)
    {
        _context = context;
    }

    // Get a single member
    public async Task<Members?> GetMemberAsync(Guid id)
    {
        var member = await _context.Member
            .FromSql($"SELECT * FROM member WHERE id = {id}")
            .FirstOrDefaultAsync();

        return member;
    }

    // Create a member
    public async Task AddAsync(Members Member)
    {
        Member.UpdatedAt = DateTime.UtcNow;
        Guid newId = Guid.NewGuid();

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO member ( id,first_name, last_name, username, email, last_active) VALUES ({newId},{Member.FirstName}, {Member.LastName}, {Member.UserName}, {Member.Email}, {Member.UpdatedAt})"
        );
    }

    // Update last email confirmation send date
    public async Task UpdateEmailConfirmationSendDate(Guid id)
    {
        DateTime updatedDateTime = DateTime.UtcNow;
         await _context.Database.ExecuteSqlAsync(
            $"UPDATE member SET last_email_confirmation_sent_date = {updatedDateTime} WHERE id = {id}"
        );
    }

    // Update email confirmation send count
    public async Task UpdateEmailConfirmationSendCount(int emailConfirmationSendCount, Guid id)
    {
        emailConfirmationSendCount++;
        var newEmailConfirmationSendCount = emailConfirmationSendCount;

        await _context.Database.ExecuteSqlAsync(
            $"UPDATE member SET email_confirmation_sent_count = {newEmailConfirmationSendCount} WHERE id = {id}"
        );
    }
}
