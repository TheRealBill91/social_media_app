using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class MemberService
{
    private readonly DataContext _context;

    public MemberService(DataContext context)
    {
        _context = context;
    }

    // Get a single member
    public async Task<Member?> GetMemberAsync(Guid id)
    {
        var member = await _context
            .Member.FromSql(
                @$"SELECT * 
                   FROM member 
                   WHERE id = {id}"
            )
            .FirstOrDefaultAsync();

        return member;
    }

    // Create a member
    public async Task AddAsync(Member member)
    {
        member.UpdatedAt = DateTime.UtcNow;
        Guid newId = Guid.NewGuid();

        await _context.Database.ExecuteSqlAsync(
            @$"INSERT INTO member 
                               (id,
                               first_name, 
                               last_name, 
                               username, 
                               email, 
                               last_active) 
                    VALUES     ({newId},
                               {member.FirstName}, 
                               {member.LastName}, 
                               {member.UserName}, 
                               {member.Email}, 
                               {member.UpdatedAt})"
        );
    }

    // Update last email confirmation send date
    public async Task UpdateEmailConfirmationSendDate(Guid id)
    {
        DateTime updatedDateTime = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET last_email_confirmation_sent_date = {updatedDateTime} 
               WHERE id = {id}"
        );
    }

    // Update email confirmation send count
    public async Task UpdateEmailConfirmationSendCount(
        int emailConfirmationSendCount,
        Guid id
    )
    {
        emailConfirmationSendCount++;
        var newEmailConfirmationSendCount = emailConfirmationSendCount;

        await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET email_confirmation_sent_count = {newEmailConfirmationSendCount} 
               WHERE id = {id}"
        );
    }

    // Update last password reset email send date
    public async Task UpdatePasswordResetSendDate(Guid id)
    {
        DateTime updatedDateTime = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET last_password_reset_email_sent_date = {updatedDateTime} 
               WHERE id = {id}"
        );
    }

    // Update password reset send count
    public async Task UpdatePasswordResetSendCount(
        int passwordResetSendCount,
        Guid id
    )
    {
        passwordResetSendCount++;
        int newEmailConfirmationSendCount = passwordResetSendCount;

        await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET password_reset_email_sent_count = {newEmailConfirmationSendCount} 
               WHERE id = {id}"
        );
    }

    // Update the date the last username email request was sent
    public async Task UpdateUsernameRequestSendDate(Guid id)
    {
        DateTime updatedDateTime = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET last_username_request_email_sent_date = {updatedDateTime} 
               WHERE id = {id}"
        );
    }

    // Update username request email send count
    public async Task UpdateUsernameRequestSendCount(
        int usernameRequestSendCount,
        Guid id
    )
    {
        usernameRequestSendCount++;
        int newUsernameRequestSendCount = usernameRequestSendCount;

        await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET username_request_email_sent_count = {newUsernameRequestSendCount} 
               WHERE id = {id}"
        );
    }

    // Checks if user has updated their username in the last 30 days,
    // if so, they must wait for 30 days to pass
    public bool CanUpdateUsername(DateTime lastUserNameUpdateDate)
    {
        var LastUsernameUpdateDate = lastUserNameUpdateDate;
        var CurrentDateTime = DateTime.UtcNow;

        var thirtyDaysPassed =
            (CurrentDateTime - LastUsernameUpdateDate) > TimeSpan.FromDays(30);

        if (thirtyDaysPassed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Updates the last date the username was updated
    public async Task UpdateLastUsernameChange(Guid id)
    {
        DateTime updatedDateTime = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET last_username_update_date = {updatedDateTime} 
               WHERE id = {id}"
        );
    }

    // Updates the members username
    public async Task<UsernameUpdateResponse> UpdateUsername(
        string newUsername,
        Guid memberId
    )
    {
        var normalizedUserName = newUsername.ToUpperInvariant();

        var id = memberId;

        var result = await _context.Database.ExecuteSqlAsync(
            @$"UPDATE member 
               SET user_name = {newUsername}, 
                   normalized_user_name = {normalizedUserName} 
               WHERE id = {id}"
        );

        if (result > 0)
        {
            return new UsernameUpdateResponse
            {
                Success = true,
                Message = "Username successfully updated"
            };
        }
        else
        {
            return new UsernameUpdateResponse
            {
                Success = false,
                Message = "Failed to update the username"
            };
        }
    }

    // checks if username already exists (and is taken)
    public async Task<bool> UsernameExists(string username)
    {
        var normalizedUserName = username.ToUpperInvariant();
        var usernameExists = await _context
            .Member.FromSql(
                @$"SELECT * 
                   FROM member 
                   WHERE normalized_user_name = {normalizedUserName}"
            )
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
}
