public class TokenBucketPolicy
{
    public int TokenLimit { get; set; }

    public int TokensPerPeriod { get; set; }

    public int ReplenishmentPeriod { get; set; } // seconds

    public int QueueLimit { get; set; }
}
