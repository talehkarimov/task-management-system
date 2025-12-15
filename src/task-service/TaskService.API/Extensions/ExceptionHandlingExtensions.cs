using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using TaskService.Application.Exceptions;

namespace TaskService.API.Extensions;

public static class ExceptionHandlingExtensions
{
    public static void UseGlobalExceptionHandling(this WebApplication app)
    {
        app.UseExceptionHandler(handler =>
        {
            handler.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                context.Response.StatusCode = exception switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    ValidationException => StatusCodes.Status400BadRequest,
                    BusinessRuleViolationException => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

                await context.Response.WriteAsJsonAsync(new
                {
                    error = exception?.Message
                });
            });
        });
    }
}
