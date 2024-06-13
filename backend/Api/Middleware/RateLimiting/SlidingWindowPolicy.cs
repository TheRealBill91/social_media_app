public class SlidingWindowPolicy
{
    public int PermitLimit { get; set; }

    public int QueueLimit { get; set; }

    public int WindowInMinutes { get; set; }

    public int SegmentsPerWindow { get; set; }
}
