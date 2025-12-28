using Serilog.Context;

namespace Common.Logging.Observability;

public static class LogScope
{
    public static IDisposable Begin(
        string component,
        string operationName,
        string? correlationId = null,
        Guid? userId = null,
        Guid? organizationId = null,
        string? eventType = null,
        Guid? messageId = null,
        Guid? eventId = null,
        Guid? outboxMessageId = null,
        int? outboxAttempt = null)
    {
        var bag = new DisposableBag();

        bag.Add(LogContext.PushProperty(LogPropertyKeys.Component, component));
        bag.Add(LogContext.PushProperty(LogPropertyKeys.OperationName, operationName));

        if (!string.IsNullOrWhiteSpace(correlationId))
            bag.Add(LogContext.PushProperty(LogPropertyKeys.CorrelationId, correlationId));

        if (userId.HasValue)
            bag.Add(LogContext.PushProperty(LogPropertyKeys.UserId, userId.Value));

        if (organizationId.HasValue)
            bag.Add(LogContext.PushProperty(LogPropertyKeys.OrganizationId, organizationId.Value));

        if (!string.IsNullOrWhiteSpace(eventType))
            bag.Add(LogContext.PushProperty(LogPropertyKeys.EventType, eventType));

        if (messageId.HasValue)
            bag.Add(LogContext.PushProperty(LogPropertyKeys.MessageId, messageId.Value));

        if (eventId.HasValue)
            bag.Add(LogContext.PushProperty(LogPropertyKeys.EventId, eventId.Value));

        if (outboxMessageId.HasValue)
            bag.Add(LogContext.PushProperty(LogPropertyKeys.OutboxMessageId, outboxMessageId.Value));

        if (outboxAttempt.HasValue)
            bag.Add(LogContext.PushProperty(LogPropertyKeys.OutboxAttempt, outboxAttempt.Value));

        return bag;
    }

    private sealed class DisposableBag : IDisposable
    {
        private readonly Stack<IDisposable> _items = new();

        public void Add(IDisposable item) => _items.Push(item);

        public void Dispose()
        {
            while (_items.Count > 0)
            {
                _items.Pop().Dispose();
            }
        }
    }
}
