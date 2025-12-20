using MediatR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using TaskService.Application.Common;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IRequestContext _context;

    public LoggingBehavior(IRequestContext context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var sw = Stopwatch.StartNew();

        using (LogContext.PushProperty(LogKeys.RequestName, requestName))
        using (LogContext.PushProperty(LogKeys.CorrelationId, _context.CorrelationId))
        using (LogContext.PushProperty(LogKeys.UserId, _context.UserId))
        using (LogContext.PushProperty(LogKeys.OrganizationId, _context.OrganizationId))
        {
            Serilog.Log.Information("Handling request");

            try
            {
                var response = await next();
                sw.Stop();

                using (LogContext.PushProperty(LogKeys.ElapsedMs, sw.ElapsedMilliseconds))
                {
                    Serilog.Log.Information("Request handled");
                }

                return response;
            }
            catch (Exception ex)
            {
                sw.Stop();
                using (LogContext.PushProperty(LogKeys.ElapsedMs, sw.ElapsedMilliseconds))
                {
                    Serilog.Log.Error(ex, "Request failed");
                }
                throw;
            }
        }
    }
}
