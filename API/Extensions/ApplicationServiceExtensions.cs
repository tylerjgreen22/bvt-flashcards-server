using Application.Core;
using Application.Flashcards;
using Application.Sets;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    // Extension method to add services to dependency injection container
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Adding DB context to service container to be injected where needed
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            // Adding cors policy to allow traffic from specified origin
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173");
                });
            });

            // Adding mediator services
            services.AddMediatR(typeof(ListSets.Handler));
            services.AddMediatR(typeof(ListFlashcards.Handler));

            // Adding auto mapper service
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);

            return services;
        }
    }
}