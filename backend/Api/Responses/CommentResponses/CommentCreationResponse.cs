public class CommentCreationResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public Guid? CommentCreationId { get; set; } // Nullable, in case the request fails
}
