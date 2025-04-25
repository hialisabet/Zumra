﻿using System.Net;
using System.Text.Json;
using FluentValidation;

namespace Zumra.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    var errors = validationException.Errors
                        .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                        .ToDictionary(
                            failureGroup => failureGroup.Key,
                            failureGroup => failureGroup.ToArray()
                        );

                    result = JsonSerializer.Serialize(new { errors });
                    break;
                default:
                    _logger.LogError(exception, "Unhandled exception");
                    result = JsonSerializer.Serialize(new { error = "An error occurred while processing your request." });
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsync(result);
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}