using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using SocialMediaApp.ConfigurationExtensions;
using SocialMediaApp.Data;
using SocialMediaApp.Extensions;
using SocialMediaApp.Infrastructure.Options;
using SocialMediaApp.RateLimiter;
using SocialMediaApp.Utils;

var builder = WebApplication.CreateBuilder(args);

var connection = string.Empty;

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    connection = builder.Configuration["POSTGRES_CONNECTION_STRING"];

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
}
else
{
    connection = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
}

builder.Services.AddCustomRateLimiting(builder.Configuration);

builder.Services.AddHttpLogging(builder.Environment);

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
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder
    .Services
    .Configure<DataProtectionTokenProviderOptions>(options =>
    {
        options.TokenLifespan = TimeSpan.FromHours(3);
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServices(builder.Environment, builder.Configuration);

builder
    .Services
    .Configure<PasswordHasherOptions>(option =>
    {
        option.IterationCount = 210000;
    });

builder.Services.AddModelServices();
builder.Services.AddConfigurationOptions(builder.Configuration);

var app = builder.Build();

app.UseHostFiltering();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    }
);

app.UseHttpsRedirection();

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
