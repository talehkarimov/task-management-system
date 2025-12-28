using Common.Logging.Observability;
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

        using (LogContext.PushProperty(LogKeys.Component, "API"))
        using (LogContext.PushProperty(LogKeys.OperationName, "HttpRequest"))
        using (LogContext.PushProperty(LogKeys.CorrelationId, correlationId))
        using (LogContext.PushProperty(LogKeys.HttpMethod, context.Request.Method))
        using (LogContext.PushProperty(LogKeys.HttpPath, context.Request.Path.Value))
        {
            try
            {
                await _next(context);

                sw.Stop();
                using (LogContext.PushProperty(LogKeys.ElapsedMs, sw.ElapsedMilliseconds))
                using (LogContext.PushProperty(LogKeys.StatusCode, context.Response.StatusCode))
                using (LogContext.PushProperty(LogKeys.Outcome, LogOutcome.Success))
                {
                    Serilog.Log.Information("HTTP request completed");
                }
            }
            catch (Exception ex)
            {
                sw.Stop();
                using (LogContext.PushProperty(LogKeys.ElapsedMs, sw.ElapsedMilliseconds))
                using (LogContext.PushProperty(LogKeys.Outcome, LogOutcome.Failure))
                {
                    Serilog.Log.Error(ex, "HTTP request failed");
                }
                throw;
            }
        }
    }
}
