using Application.Exceptions;
using DocumentFormat.OpenXml.InkML;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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


                object response;
                int statusCode;


                switch (ex)
                {
                    case ValidationException validationEx:
                        statusCode = StatusCodes.Status400BadRequest;
                        response = new
                        {
                            success = false,
                            message = "Validation failed.",
                            statusCode,
                            errors = validationEx.Errors
                                .GroupBy(e => e.PropertyName)
                                .Select(g => new
                                {
                                    field = g.Key,
                                    errors = g.Select(e => e.ErrorMessage).ToArray()
                                })
                        };
                        break;

                    case DbUpdateException dbEx:
                        statusCode = StatusCodes.Status500InternalServerError;
                        response = new
                        {
                            success = false,
                            error = "A database error occurred. Please try again later.",
                            statusCode
                        };
                        break;

                    case SecurityTokenExpiredException tokenExpired:
                        statusCode = StatusCodes.Status401Unauthorized;
                        response = new
                        {
                            success = false,
                            error = "Token has expired.",
                            statusCode
                        };
                        break;

                    case SecurityTokenException tokenInvalid:
                        statusCode = StatusCodes.Status401Unauthorized;
                        response = new
                        {
                            success = false,
                            error = "Invalid token.",
                            statusCode
                        };
                        break;

                    case NotFoundException notFoundEx:
                        statusCode = StatusCodes.Status404NotFound;
                        response = new
                        {
                            success = false,
                            error = notFoundEx.Message,
                            statusCode
                        };
                        break;

                    case UnauthorizedException unauthorizedEx:
                        statusCode = StatusCodes.Status401Unauthorized;
                        response = new
                        {
                            success = false,
                            error = unauthorizedEx.Message,
                            statusCode
                        };
                        break;

                    default:
                        statusCode = (int)GetStatusCode(ex);
                        var errorMessage = ex is AppException || !_env.IsDevelopment()
                            ? GetErrorMessage(ex)
                            : ex.ToString(); // full stack trace in dev for debugging

                        response = new
                        {
                            success = false,
                            error = errorMessage,
                            statusCode
                        };
                        break;
                }

                context.Response.StatusCode = statusCode;
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
                UnauthorizedException => HttpStatusCode.Unauthorized,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                ArgumentException => HttpStatusCode.BadRequest,
                AppException => HttpStatusCode.BadRequest,
                KeyNotFoundException => HttpStatusCode.NotFound,
                NotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };
        }

        private string GetErrorMessage(Exception ex)
        {
           
            return ex switch
            {
                AppException appEx => appEx.Message,
                UnauthorizedAccessException => "Unauthorized: Invalid credentials.",
                ArgumentException => "Bad request: Invalid input.",
                KeyNotFoundException => "Resource not found.",
                _ => "An unexpected error occurred. Please try again later."
            };
        }

    }
}
