namespace SocialMediaApp.ConfigurationExtensions;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection AddConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<ApiSettingsOptions>(
            configuration.GetSection(ApiSettingsOptions.ApiSettings)
        );

        // Add other configuration options as needed
        return services;
    }
}
