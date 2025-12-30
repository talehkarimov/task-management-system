using Common.Logging.Observability;
using Microsoft.AspNetCore.Http;

namespace Common.Middlewares;

public sealed class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId =
            context.Request.Headers[HeaderNames.CorrelationId].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Items[HeaderNames.CorrelationId] = correlationId;
        context.Response.Headers[HeaderNames.CorrelationId] = correlationId;

        await _next(context);
    }
}
