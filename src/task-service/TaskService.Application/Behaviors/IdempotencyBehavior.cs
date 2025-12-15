using MediatR;
using TaskService.Application.Exceptions;
using TaskService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace TaskService.Application.Behaviors;

public sealed class IdempotencyBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IIdempotencyService _idempotencyService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdempotencyBehavior(
        IIdempotencyService idempotencyService,
        IHttpContextAccessor httpContextAccessor)
    {
        _idempotencyService = idempotencyService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestId = _httpContextAccessor.HttpContext?
            .Request.Headers["X-Request-Id"]
            .ToString();

        if (string.IsNullOrWhiteSpace(requestId))
        {
            return await next();
        }

        if (await _idempotencyService
            .IsRequestProcessedAsync(requestId, cancellationToken))
        {
            throw new BusinessRuleViolationException(
                "Duplicate request detected.");
        }

        var response = await next();

        await _idempotencyService
            .MarkRequestAsProcessedAsync(requestId, cancellationToken);

        return response;
    }
}
