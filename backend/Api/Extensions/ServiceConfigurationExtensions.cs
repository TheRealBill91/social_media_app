namespace SocialMediaApp.ConfigurationExtensions;

public static class OptionsPatternExtensions
{
    /// <summary>
    /// Configures application-specific options bound to configuration sections.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The configuration to use for binding options.</param>
    /// <returns>The IServiceCollection for chaining further configuration.</returns>
    public static IServiceCollection AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<ApiSettingsOptions>(
            configuration.GetSection(ApiSettingsOptions.ApiSettings)
        );

        return services;
    }
}
