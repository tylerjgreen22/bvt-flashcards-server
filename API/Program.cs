using API.Extensions;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Adding service via extension method
builder.Services.AddApplicationService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adding CORS policy
app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

// Obtaining the service container from scope
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

// Obtaining the context from the service container to pass to the Seed.SeedAsync method to seed the database
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedAsync(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migrations");
}

app.Run();
