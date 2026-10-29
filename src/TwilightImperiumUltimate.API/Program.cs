using Serilog;
using TwilightImperiumUltimate.API.Hubs;
using TwilightImperiumUltimate.API.Services;
using TwilightImperiumUltimate.Core.Entities.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices(builder.Configuration);
builder.Services.RegisterOptions(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseRouting();
app.UseRateLimiter();

var allowedOrigins = new[] { app.Configuration["Frontend:Url"], app.Configuration["Frontend:LocalUrl"], app.Configuration["Frontend:LocalUrlIisExpress"], app.Configuration["Frontend:ProductionUrl"], app.Configuration["Frontend:WwwUrl"] }
    .Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Select(origin => origin!.TrimEnd('/'))
    .Distinct()
    .ToArray();

app.UseCors(builder => builder
    .WithOrigins(allowedOrigins)
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
app.MapIdentityApi<TwilightImperiumUser>();
app.MapHub<DraftRoomHub>("/hubs/draft-rooms");

Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");

if (!app.Environment.IsEnvironment("Testing"))
{
    await app.CreateOrUpdateDbContextAsync();
}

await app.AppRunAsync();

public partial class Program
{
    protected Program()
    {
    }
}
