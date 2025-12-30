using Common.Logging.Observability;
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System.Diagnostics;

namespace Common.Middlewares;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var correlationId = context.Items[HeaderNames.CorrelationId]?.ToString();

        using (LogContext.PushProperty(LogPropertyKeys.Component, "API"))
        using (LogContext.PushProperty(LogPropertyKeys.OperationName, "HttpRequest"))
        using (LogContext.PushProperty(LogPropertyKeys.CorrelationId, correlationId))
        using (LogContext.PushProperty(LogPropertyKeys.HttpMethod, context.Request.Method))
        using (LogContext.PushProperty(LogPropertyKeys.HttpPath, context.Request.Path))
        {
            try
            {
                await _next(context);

                sw.Stop();
                using (LogContext.PushProperty(LogPropertyKeys.StatusCode, context.Response.StatusCode))
                using (LogContext.PushProperty(LogPropertyKeys.ElapsedMs, sw.ElapsedMilliseconds))
                using (LogContext.PushProperty(LogPropertyKeys.Outcome, LogOutcome.Success))
                {
                    Serilog.Log.Information("HTTP request completed");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                using (LogContext.PushProperty(LogPropertyKeys.Outcome, LogOutcome.Failure))
                {
                    Serilog.Log.Error(ex, "HTTP request failed");
                }
                throw;
            }
        }
    }
}
