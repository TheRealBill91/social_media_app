namespace SocialMediaApp.Services;

public static class ServiceModelRegistration
{
    public static void AddModelServices(this IServiceCollection services)
    {
        services.AddScoped<PostService>();
        services.AddScoped<MemberService>();
        services.AddScoped<AuthService>();
    }
}
