using AuditService.Application.Records;
using AuditService.Infrastructure.Persistence;
using Common.Logging.Observability;
using Common.Messaging;
using Common.Messaging.IntegrationEvents.TaskService;
using MassTransit;
using System.Text.Json;
using LogContext = Serilog.Context.LogContext;
namespace AuditService.Infrastructure.Consumers;

public sealed class TaskCommentAddedConsumer(AuditDbContext dbContext) : IConsumer<TaskCommentAddedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskCommentAddedIntegrationEventV1> context)
    {
        var correlationId = ConsumerObservability.ResolveCorrelationId(context);
        var operationName = $"Consume:{nameof(TaskCommentAddedIntegrationEventV1)}";
        var outboxMessageId = ConsumerObservability.ResolveOutboxMessageId(context);

        using (LogContext.PushProperty(LogPropertyKeys.Component, "Consumer"))
        using (LogContext.PushProperty(LogPropertyKeys.OperationName, operationName))
        using (LogContext.PushProperty(LogPropertyKeys.EventType, nameof(TaskCommentAddedIntegrationEventV1)))
        using (LogContext.PushProperty(LogPropertyKeys.EventId, context.Message.EventId))
        using (LogContext.PushProperty(LogPropertyKeys.MessageId, context.MessageId))
        using (LogContext.PushProperty(LogPropertyKeys.OutboxMessageId, outboxMessageId))
        using (LogContext.PushProperty(LogPropertyKeys.CorrelationId, correlationId))
        using (LogContext.PushProperty(LogPropertyKeys.UserId, context.Message.CommentedByUserId))
        {
            var record = new AuditRecord
            {
                Id = Guid.NewGuid(),
                ServiceName = "TaskService",
                EventType = nameof(TaskCommentAddedIntegrationEventV1),
                EntityId = context.Message.EventId,
                UserId = context.Message.CommentedByUserId,
                Payload = JsonSerializer.Serialize(context.Message),
                OccurredOn = context.Message.OccurredOn,
                CorrelationId = context.CorrelationId?.ToString()
                                ?? string.Empty
            };

            dbContext.AuditRecords.Add(record);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }
}
