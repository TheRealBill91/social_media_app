using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.DTOs;

namespace SocialMediaApp.Services;

public class MemberProfileService
{
    private readonly DataContext _context;

    public MemberProfileService(DataContext context)
    {
        _context = context;
    }

    // create the member profile
    public async Task<MemberProfileCreationResponse> CreateMemberProfile(
        Guid memberId
    )
    {
        var createdAt = DateTime.UtcNow;
        var updatedAt = createdAt;
        var profileCreationResult = await _context.Database.ExecuteSqlAsync(
            @$"INSERT INTO member_profile 
                               (member_id, 
                               photo_url, 
                               bio, 
                               location, 
                               url, 
                               created_at, 
                               updated_at, 
                               deleted_at) 
                   VALUES      ({memberId}, 
                               null, 
                               null, 
                               null, 
                               null, 
                               {createdAt}, 
                               {updatedAt}, 
                               null)"
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

    public async Task<MemberProfileInfoDTO?> GetMemberProfile(Guid memberId)
    {
        return await _context
            .Database.SqlQuery<MemberProfileInfoDTO>(
                @$"SELECT member.user_name,
                      member_profile.photo_url,
                      member_profile.bio,
                      member_profile.location,
                      member_profile.url,
                      member_profile.created_at
                   FROM member_profile
                   LEFT JOIN member ON member_profile.member_id = member.id
                   WHERE member_profile.member_id = {memberId} AND member_profile.deleted_at IS NULL"
            )
            .FirstOrDefaultAsync();
    }

    public async Task<MemberProfileUpdateResponse> UpdateMemberProfile(
        Guid memberId,
        MemberProfileUpdateDTO updatedInfo
    )
    {
        var updateResponse = new MemberProfileUpdateResponse();

        var member = await _context.Member.FirstOrDefaultAsync(m =>
            m.Id == memberId
        );
        if (member == null)
        {
            updateResponse.Success = false;
            updateResponse.Message = "Cannot find the member";
            return updateResponse;
        }

        member.FirstName = updatedInfo.FirstName;
        member.LastName = updatedInfo.LastName;
        member.UserName = updatedInfo.UserName;
        member.NormalizedUserName = updatedInfo.UserName.ToUpperInvariant();
        member.UpdatedAt = DateTime.UtcNow;

        var memberProfile = await _context.MemberProfile.FirstOrDefaultAsync(
            mp => mp.MemberId == memberId
        );
        if (memberProfile == null)
        {
            updateResponse.Success = false;
            updateResponse.Message = "Cannot find the member profile";
            return updateResponse;
        }

        memberProfile.PhotoURL = updatedInfo.Photo_url;
        memberProfile.Bio = updatedInfo.Bio;
        memberProfile.Location = updatedInfo.Location;
        memberProfile.URL = updatedInfo.Url;
        memberProfile.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        updateResponse.Success = true;
        return updateResponse;
    }
}
