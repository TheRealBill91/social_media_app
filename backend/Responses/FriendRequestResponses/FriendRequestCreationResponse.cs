public class FriendRequestCreationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = null!;
    public Guid? FriendRequestCreationId { get; set; } // Nullable, in case the request fails
}
