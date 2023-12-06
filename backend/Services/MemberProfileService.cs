using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services;

public class MemberProfileService
{
    private readonly DataContext _context;

    public MemberProfileService(DataContext context)
    {
        _context = context;
    }

    // create the member profile
    public async Task<MemberProfileCreationResponse> CreateMemberProfile(Guid memberId)
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var profileCreationResult = await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO member_profile (member_id, photo_url, bio, location, url, created_at, updated_at, deleted_at) VALUES ({memberId}, null, null, null, null, {createdAt}, {updatedAt}, null)"
        );

        if (profileCreationResult > 0)
        {
            return new MemberProfileCreationResponse
            {
                Success = true,
                Message = "Member Profile successfully created"
            };
        }
        else
        {
            return new MemberProfileCreationResponse
            {
                Success = false,
                Message = "Failed to create the member profile"
            };
        }
    }

    public async Task<MemberProfileInfoResponse?> GetMemberProfile(Guid memberId)
    {
        var memberProfile = await _context.MemberProfile
            .Where(mp => mp.MemberId == memberId && mp.DeletedAt == null)
            .Join(
                _context.Member,
                memberProfile => memberProfile.MemberId,
                member => member.Id,
                (memberProfile, member) => new { MemberProfile = memberProfile, Member = member }
            )
            .Select(
                mp =>
                    new MemberProfileInfoResponse
                    {
                        FirstName = mp.Member.FirstName,
                        LastName = mp.Member.LastName,
                        UserName = mp.Member.UserName!,
                        Photo_url = mp.MemberProfile.PhotoURL,
                        Bio = mp.MemberProfile.Bio,
                        Location = mp.MemberProfile.Location,
                        Url = mp.MemberProfile.URL,
                        CreatedAt = mp.MemberProfile.CreatedAt
                    }
            )
            .FirstOrDefaultAsync();

        return memberProfile;
    }
}
