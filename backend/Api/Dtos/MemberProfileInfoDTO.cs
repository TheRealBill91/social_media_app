using Microsoft.EntityFrameworkCore;

namespace SocialMediaApp.DTOs;

[Keyless]
public class MemberProfileInfoDTO
{
    public string UserName { get; set; } = null!;
    public string? Photo_url { get; set; }
    public string? Bio { get; set; }

    public string? Location { get; set; }

    public string? Url { get; set; }

    public DateTime CreatedAt { get; set; }
}
