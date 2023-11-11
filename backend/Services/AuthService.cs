



using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Services;


public class AuthService 
{
    private readonly DataContext _context;

    public AuthService(DataContext context)
    {
        _context = context;
    }


    public async Task UpdateUserLastActivityDateAsync(Guid userId)
    {
        var lastActivityDate = DateTime.UtcNow;
        await _context.Database.ExecuteSqlAsync(
            $"UPDATE members SET updated_at = {lastActivityDate} WHERE id = {userId} "
        );

    }

    
}