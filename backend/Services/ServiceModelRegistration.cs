namespace SocialMediaApp.Services;

using Microsoft.AspNetCore.Identity.UI.Services;
using SocialMediaApp.Models;

public static class ServiceModelRegistration
{
    public static void AddModelServices(this IServiceCollection services)
    {
        services.AddScoped<PostService>();
        services.AddScoped<MemberService>();
        services.AddScoped<AuthService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<CustomEmailConfirmationTokenProvider<Member>>();
    }
}
