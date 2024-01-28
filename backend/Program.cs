using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using SocialMediaApp.Data;
using SocialMediaApp.Models;
using SocialMediaApp.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = string.Empty;

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    connection = builder.Configuration["POSTGRES_CONNECTION_STRING"];
}
else
{
    connection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
}

builder.Services.AddCustomRateLimiting(builder.Configuration);

if (builder.Environment.IsDevelopment())
{
    builder
        .Services
        .AddHttpLogging(logging =>
        {
            logging.LoggingFields =
                HttpLoggingFields.RequestMethod
                | HttpLoggingFields.RequestPath
                | HttpLoggingFields.RequestProtocol
                | HttpLoggingFields.RequestHeaders
                | HttpLoggingFields.ResponseStatusCode
                | HttpLoggingFields.ResponseHeaders
                | HttpLoggingFields.ResponseBody;
            logging.RequestHeaders.Add("sec-ch-ua");
            logging.ResponseHeaders.Add("MyResponseHeader");
        });
}
else if (builder.Environment.IsProduction())
{
    builder
        .Services
        .AddHttpLogging(
            logging =>
                logging.LoggingFields =
                    HttpLoggingFields.RequestPath | HttpLoggingFields.ResponseStatusCode
        );
}

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connection);
dataSourceBuilder.MapEnum<FriendRequestStatus>();
var dataSource = dataSourceBuilder.Build();

builder
    .Services
    .AddDbContext<DataContext>(
        options =>
            options
                .UseNpgsql(dataSource)
                .ReplaceService<IHistoryRepository, CamelCaseHistoryContext>()
                .UseSnakeCaseNamingConvention()
    );

builder
    .Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddJsonOptions(options =>
    {
        // Serializes enum's to string's
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder
    .Services
    .Configure<DataProtectionTokenProviderOptions>(options =>
    {
        options.TokenLifespan = TimeSpan.FromHours(3);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .Services
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
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.SignIn.RequireConfirmedEmail = true;

        options.User.RequireUniqueEmail = true;

        options
            .Tokens
            .ProviderMap
            .Add(
                "CustomEmailConfirmation",
                new TokenProviderDescriptor(typeof(CustomEmailConfirmationTokenProvider<Member>))
            );
        options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddModelServices();

string? googleClientId = builder.Environment.IsDevelopment()
    ? builder.Configuration["Authentication:Google:ClientId"]
    : Environment.GetEnvironmentVariable("Authentication:Google:ClientId");

string? googleClientSecret = builder.Environment.IsDevelopment()
    ? builder.Configuration["Authentication:Google:ClientSecret"]
    : Environment.GetEnvironmentVariable("Authentication:Google:ClientSecret");

builder
    .Services
    .AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = googleClientId!;
        googleOptions.ClientSecret = googleClientSecret!;
        googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        googleOptions.CallbackPath = new PathString("/signin-google");
    });

builder
    .Services
    .ConfigureApplicationCookie(options =>
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

        if (builder.Environment.IsProduction())
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        }
        else if (builder.Environment.IsDevelopment())
        {
            options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        }
    });

builder
    .Services
    .Configure<PasswordHasherOptions>(option =>
    {
        option.IterationCount = 210000;
    });

var app = builder.Build();

app.UseHostFiltering();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

app.UseHttpLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
    app.UseHsts();
    app.UseExceptionHandler("/error");
}

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
