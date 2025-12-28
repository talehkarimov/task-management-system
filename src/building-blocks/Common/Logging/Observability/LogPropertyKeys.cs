namespace Common.Logging.Observability;

public static class LogPropertyKeys
{
    public const string ServiceName = "ServiceName";
    public const string CorrelationId = "CorrelationId";

    public const string Component = "Component";        
    public const string OperationName = "OperationName";
    public const string Outcome = "Outcome";            

    public const string UserId = "UserId";
    public const string OrganizationId = "OrganizationId";

    public const string RequestName = "RequestName";
    public const string ElapsedMs = "ElapsedMs";

    public const string HttpMethod = "HttpMethod";
    public const string HttpPath = "HttpPath";
    public const string StatusCode = "StatusCode";

    public const string EventType = "EventType";
    public const string MessageId = "MessageId";
    public const string EventId = "EventId";
    public const string OutboxMessageId = "OutboxMessageId";
    public const string OutboxAttempt = "OutboxAttempt";
}
