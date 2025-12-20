namespace TaskService.Application.Common;

public static class LogKeys
{
    public const string CorrelationId = "CorrelationId";
    public const string UserId = "UserId";
    public const string OrganizationId = "OrganizationId";

    public const string RequestName = "RequestName";
    public const string ElapsedMs = "ElapsedMs";

    public const string OutboxMessageId = "OutboxMessageId";
    public const string OutboxAttempt = "OutboxAttempt";
    public const string EventType = "EventType";
}
