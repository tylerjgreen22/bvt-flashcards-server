using System.Text;
using API.Services;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;

namespace API.Extensions
{
    // Extension class to add identity services
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {
            // Adding Identity service and configure options
            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<DataContext>();

            // Key used to decrypt incoming tokens
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

            // Adding JWT authentication and configuring incoming token validation requirements
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // Add authorization policy to prevent users from modifying resource they dont own
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsOwner", policy =>
                {
                    policy.Requirements.Add(new IsOwnerRequirement());
                });
            });

            // Add IsOwnerRequirementHandler to prevent users from modifying resource that are not theirs
            services.AddTransient<IAuthorizationHandler, IsOwnerRequirementHandler>();

            // Adding token service to be injected throughout application
            services.AddScoped<TokenService>();

            return services;
        }
    }
}