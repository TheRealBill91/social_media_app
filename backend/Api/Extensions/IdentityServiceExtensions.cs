using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using SocialMediaApp.Data;
using SocialMediaApp.Models;

namespace SocialMediaApp.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IWebHostEnvironment env,
        IConfiguration configuration
    )
    {
        services
            .AddIdentity<Member, IdentityRole<Guid>>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(
                    30
                );
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.SignIn.RequireConfirmedEmail = true;

                options.User.RequireUniqueEmail = true;

                options.Tokens.ProviderMap.Add(
                    "CustomEmailConfirmation",
                    new TokenProviderDescriptor(
                        typeof(CustomEmailConfirmationTokenProvider<Member>)
                    )
                );
                options.Tokens.EmailConfirmationTokenProvider =
                    "CustomEmailConfirmation";
            })
            .AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        var googleClientId = env.IsDevelopment()
            ? configuration["Authentication:Google:ClientId"]
            : Environment.GetEnvironmentVariable(
                "Authentication:Google:ClientId"
            );

        var googleClientSecret = env.IsDevelopment()
            ? configuration["Authentication:Google:ClientSecret"]
            : Environment.GetEnvironmentVariable(
                "Authentication:Google:ClientSecret"
            );

        services
            .AddAuthentication()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = googleClientId!;
                googleOptions.ClientSecret = googleClientSecret!;
                googleOptions.ClaimActions.MapJsonKey(
                    "urn:google:picture",
                    "picture",
                    "url"
                );
                googleOptions.CallbackPath = new PathString("/signin-google");

                // This is a workaround in order to allow users
                // to select their google account on each google login
                // https://github.com/dotnet/aspnetcore/issues/47054#issuecomment-1786192809
                googleOptions.Events = new OAuthEvents()
                {
                    OnRedirectToAuthorizationEndpoint = c =>
                    {
                        c.RedirectUri += "&prompt=select_account";
                        c.Response.Redirect(c.RedirectUri);
                        return Task.CompletedTask;
                    }
                };
            });

        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };

            options.ExpireTimeSpan = TimeSpan.FromDays(3);
            options.SlidingExpiration = true;

            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.Name = "auth";
            options.SlidingExpiration = true;

            if (env.IsProduction())
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }
            else if (env.IsDevelopment())
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            }
        });

        return services;
    }
}
