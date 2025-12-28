using Common.Logging.Observability;

namespace TaskService.Application.Common;

public static class LogKeys
{
    public const string CorrelationId = LogPropertyKeys.CorrelationId;
    public const string UserId = LogPropertyKeys.UserId;
    public const string OrganizationId = LogPropertyKeys.OrganizationId;

    public const string Component = LogPropertyKeys.Component;
    public const string OperationName = LogPropertyKeys.OperationName;
    public const string Outcome = LogPropertyKeys.Outcome;

    public const string RequestName = LogPropertyKeys.RequestName;
    public const string ElapsedMs = LogPropertyKeys.ElapsedMs;

    public const string HttpMethod = LogPropertyKeys.HttpMethod;
    public const string HttpPath = LogPropertyKeys.HttpPath;
    public const string StatusCode = LogPropertyKeys.StatusCode;

    public const string OutboxMessageId = LogPropertyKeys.OutboxMessageId;
    public const string OutboxAttempt = LogPropertyKeys.OutboxAttempt;
    public const string EventType = LogPropertyKeys.EventType;
}
