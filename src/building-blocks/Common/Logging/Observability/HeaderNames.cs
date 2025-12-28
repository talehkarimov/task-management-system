namespace Common.Logging.Observability;

public static class HeaderNames
{
    public const string CorrelationId = "X-Correlation-Id";
    public const string OutboxMessageId = "X-Outbox-Message-Id";
    public const string UserId = "X-User-Id";
    public const string OrganizationId = "X-Org-Id";
}
