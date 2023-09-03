using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    // Check if the user making the request is the ownwer of that resource on authorized routes
    public class IsOwnerRequirement : IAuthorizationRequirement { }

    // Handler for IsOwnerRequirement
    public class IsOwnerRequirementHandler : AuthorizationHandler<IsOwnerRequirement>
    {
        // Injecting the db context and the http context accessor via constructor
        private readonly DataContext _dbcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IsOwnerRequirementHandler(DataContext dbcontext, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbcontext = dbcontext;
        }

        // Override of the HandleRequirementAsync method from the AuthorizationHandler class
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement)
        {
            // Find the user making the request using the name identifier claim from the token. If not found returns Task with no completion (Forbidden)
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Task.CompletedTask;

            // Gets the set id from the request route values, pulled from the http context
            var setId = _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString();

            // Find the set that is being modified, includes the app user. If not found return null.
            var set = _dbcontext.Sets.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == setId).Result;
            if (set == null) return Task.CompletedTask;

            // If the app user id on the set matches the user id from the token, add success to the requirement.
            if (set.AppUserId == userId) context.Succeed(requirement);

            // Return completed task. If the previous check passed, this will allow the user to make the change to the resource
            return Task.CompletedTask;
        }
    }
}