using System.Net;
using System.Text.Json;
using Application.Core;

namespace API.Middleware
{
    // Middleware that catches exception during the request processing, and returns a JSON error based on environment
    public class ExceptionMiddleware
    {
        // Injecting required services via constructor
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }

        // Method that catches error in http context. If no error, moves on to next request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Logs error
                _logger.LogError(ex, ex.Message);

                // Sets context response to JSON and sets the status code to internal server error
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // If the app is in dev, returns custom AppException with the status code, message and stack trace, otherwise just returns status code and "Internal server error"
                var response = _env.IsDevelopment() ? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString()) :
                    new AppException(context.Response.StatusCode, "Internal Server Error");

                // Set JSON serializer options to camel case naming policy
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                // Create json reponse using response object and options
                var json = JsonSerializer.Serialize(response, options);

                // Write to context the created JSON response
                await context.Response.WriteAsync(json);
            }
        }
    }
}