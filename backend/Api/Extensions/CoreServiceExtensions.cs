using Microsoft.AspNetCore.Identity.UI.Services;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

namespace SocialMediaApp.Extensions;

public static class ServiceModelRegistration
{
    public static void AddModelServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<CommentService>();
        services.AddScoped<CommentUpvoteService>();

        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<CustomEmailConfirmationTokenProvider<Member>>();

        services.AddScoped<FriendRequestService>();
        services.AddScoped<FriendshipService>();
        services.AddScoped<MemberProfileService>();
        services.AddScoped<MemberService>();
        services.AddScoped<PostService>();
        services.AddScoped<PostUpvoteService>();
    }
}
