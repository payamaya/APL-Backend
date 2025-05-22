using Application.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Application.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly IHostEnvironment _env;
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(IHostEnvironment env, RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _env = env;
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Proceed with request pipeline
                if (context.Response.StatusCode == 401)
                {
                    await HandleUnauthorizedAsync(context);
                }
                else if (context.Response.StatusCode == 403)
                {
                    await HandleForbiddenAsync(context);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)GetStatusCode(ex);

                var errorMessage = _env.IsDevelopment()
                    ? ex.ToString()
                    : GetErrorMessage(ex);

                var response = new
                {
                    success = false,
                    error = errorMessage,
                    statusCode = context.Response.StatusCode
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
        private async Task HandleUnauthorizedAsync(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    error = "Unauthorized. Please log in.",
                    statusCode = 401
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
        private async Task HandleForbiddenAsync(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    error = "Forbidden. You are not allowed to access this resource.",
                    statusCode = 403
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }

        private HttpStatusCode GetStatusCode(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException => HttpStatusCode.BadRequest,
                AppException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private string GetErrorMessage(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => "Unauthorized: Invalid credentials.",
                ArgumentException => "Bad request: Invalid input.",
                KeyNotFoundException => "Resource not found.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

    }
}
