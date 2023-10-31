using SocialMediaApp.Models;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;

namespace SocialMediaApp.Services;

public class UserService
{
    private readonly DataContext _context;

    public UserService(DataContext context)
    {
        _context = context;
    }

    // Get a single user
    public async Task<Users> GetByIdAsync(int id) => await _context.User.FindAsync(id);

    // Create a user
    public async Task AddAsync(Users User)
    {
        User.LastActive = DateTime.Now;

        await _context.Database.ExecuteSqlAsync(
            $"INSERT INTO Users (FirstName, LastName, UserName, Email, LastActive) VALUES ({User.FirstName}, {User.LastName}, {User.UserName}, {User.Email}, {User.LastActive})"
        );
    }
}
