using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;


namespace Infrastructure.Security
{
    // User accessor for gaining access to the http context accessor from layers that do not have access (Application)
    public class UserAccessor : IUserAccessor
    {
        // Injecting the http context accessor via the constructor
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Method that gets the username from the token claims
        public string GetUsername()
        {
            // Check auth header is present on request. If missing return null
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader)) return null;

            // Pull the username from the token claims via http context and return it
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}