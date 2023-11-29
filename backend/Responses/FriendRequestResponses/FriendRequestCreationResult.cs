public class FriendRequestCreationResult
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public Guid? FriendRequestCreationId { get; set; } // Nullable, in case the request fails
}
