public class FriendshipCreationResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public Guid? FriendshipCreationId { get; set; } // Nullable, in case the request fails
}
