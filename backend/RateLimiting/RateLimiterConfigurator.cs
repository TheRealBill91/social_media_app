using Microsoft.AspNetCore.RateLimiting;

public static class RateLimiterConfigurator
{
    public static IServiceCollection AddCustomRateLimiting(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // load policy options from each policy file
        var generalFixedWindowOptions = new GeneralFixedWindowPolicy();
        configuration.GetSection("RateLimiting:GeneralFixed").Bind(generalFixedWindowOptions);

        // add rate limiter policy method for each configuration

        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter(
                "GeneralFixed",
                limiterOptions =>
                {
                    limiterOptions.PermitLimit = generalFixedWindowOptions.PermitLimit;
                    limiterOptions.Window = generalFixedWindowOptions.WindowInSeconds;
                    limiterOptions.QueueLimit = generalFixedWindowOptions.QueueLimit;
                }
            );
        });

        return services;
    }
}
