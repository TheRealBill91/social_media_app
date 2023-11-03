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
    public async Task<Members> GetByIdAsync(int id) => await _context.Member.FindAsync(id);

    // Create a member
    public async Task AddAsync(Members Member)
    {
        Member.LastActive = DateTime.UtcNow;
        Guid newId = Guid.NewGuid();

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO member ( id,first_name, last_name, username, email, last_active) VALUES ({newId},{Member.FirstName}, {Member.LastName}, {Member.UserName}, {Member.Email}, {Member.LastActive})"
        );
    }
}
