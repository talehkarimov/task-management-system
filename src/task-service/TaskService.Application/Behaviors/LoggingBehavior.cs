using MediatR;
using Microsoft.Extensions.Logging;

namespace TaskService.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling command {CommandName} with data {@Command}",
            typeof(TRequest).Name,
            request);

        var response = await next();

        _logger.LogInformation(
            "Command {CommandName} handled successfully",
            typeof(TRequest).Name);

        return response;
    }
}
