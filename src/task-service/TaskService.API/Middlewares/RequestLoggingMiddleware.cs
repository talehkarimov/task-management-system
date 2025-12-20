using Serilog.Context;
using System.Diagnostics;
using TaskService.API.Middlewares;
using TaskService.Application.Common;

namespace TaskService.API.Middlewares;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    public RequestLoggingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var correlationId = context.Items[CorrelationIdMiddleware.HeaderName]?.ToString();

        using (LogContext.PushProperty(LogKeys.CorrelationId, correlationId))
        using (LogContext.PushProperty("HttpMethod", context.Request.Method))
        using (LogContext.PushProperty("HttpPath", context.Request.Path.Value))
        {
            try
            {
                await _next(context);

                sw.Stop();
                using (LogContext.PushProperty(LogKeys.ElapsedMs, sw.ElapsedMilliseconds))
                using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
                {
                    Serilog.Log.Information("HTTP request completed");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                using (LogContext.PushProperty(LogKeys.ElapsedMs, sw.ElapsedMilliseconds))
                {
                    Serilog.Log.Error(ex, "HTTP request failed");
                }
                throw;
            }
        }
    }
}
