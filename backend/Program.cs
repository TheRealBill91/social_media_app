using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
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

builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(connection));
builder.Services.AddModelServices();
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error-development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
    app.UseExceptionHandler("/error");
}

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
