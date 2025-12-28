using Common.Logging.Observability;
using MassTransit;

namespace NotificationService.Infrastructure.Consumers;

internal static class ConsumerObservability
{
    public static string? ResolveCorrelationId(ConsumeContext context)
    {
        if (context.Headers.TryGetHeader(HeaderNames.CorrelationId, out var value) && value is not null)
            return value.ToString();

        return context.CorrelationId?.ToString();
    }

    public static Guid? ResolveOutboxMessageId(ConsumeContext context)
    {
        if (!context.Headers.TryGetHeader(HeaderNames.OutboxMessageId, out var value) || value is null)
            return null;

        return Guid.TryParse(value.ToString(), out var id) ? id : null;
    }
}
