public class FriendRequestAcceptResult
{
    public bool Success { get; set; }
    public required string Message { get; set; }
    public Guid? FriendshipAcceptId { get; set; } // Id of newly created friendship, nullable, in case the request fails
}
