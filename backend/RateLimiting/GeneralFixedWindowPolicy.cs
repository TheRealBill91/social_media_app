public class GeneralFixedWindowPolicy
{
    public int PermitLimit { get; set; }
    public TimeSpan WindowInSeconds { get; set; }

    // max number of waiting requests that can be queued if
    // rate limit is exceeded
    public int QueueLimit { get; set; }
}
