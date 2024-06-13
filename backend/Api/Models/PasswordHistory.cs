using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.Models;

[PrimaryKey(nameof(Id))]
public class PasswordHistory
{
    public Guid Id { get; set; }

    public string PasswordHash { get; set; } = null!;
}
