public class PostCreationResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public Guid? PostCreationId { get; set; } // Nullable, in case the request fails
}
