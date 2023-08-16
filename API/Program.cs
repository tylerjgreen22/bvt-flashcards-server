using API.Extensions;
using API.Middleware;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Adding services via extension methods
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

// Add exception handler middleware
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adding CORS policy
app.UseCors("CorsPolicy");

// Adding authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Obtaining the service container from scope
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// Obtaining the context and userManager from the service container to pass to the Seed.SeedAsync method to seed the database
try
{
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    await Seed.SeedAsync(context, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migrations");
}

app.Run();
