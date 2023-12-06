using System.Numerics;
using Microsoft.EntityFrameworkCore;

[Keyless]
public class MemberProfileInfoResponse
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;
    public string? Photo_url { get; set; }
    public string? Bio { get; set; }

    public string? Location { get; set; }

    public string? Url { get; set; }

    public DateTime CreatedAt { get; set; }
}
