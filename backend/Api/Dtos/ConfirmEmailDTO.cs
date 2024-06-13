namespace SocialMediaApp.Models;

public class ConfirmEmailDTO
{
    public Guid UserId { get; set; }

    public string Code { get; set; } = null!;
}
