using AuditService.Application.Records;
using AuditService.Infrastructure.Persistence;
using Common.Logging.Observability;
using Common.Messaging;
using Common.Messaging.IntegrationEvents.TaskService;
using MassTransit;
using Serilog;
using System.Text.Json;
using LogContext = Serilog.Context.LogContext;

namespace AuditService.Infrastructure.Consumers;

public sealed class TaskBlockedConsumer(AuditDbContext dbContext) : IConsumer<TaskBlockedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskBlockedIntegrationEventV1> context)
    {
        var correlationId = ConsumerObservability.ResolveCorrelationId(context);
        var operationName = $"Consume:{nameof(TaskBlockedIntegrationEventV1)}";
        var outboxMessageId = ConsumerObservability.ResolveOutboxMessageId(context);

        using (LogContext.PushProperty(LogPropertyKeys.Component, "Consumer"))
        using (LogContext.PushProperty(LogPropertyKeys.OperationName, operationName))
        using (LogContext.PushProperty(LogPropertyKeys.EventType, nameof(TaskBlockedIntegrationEventV1)))
        using (LogContext.PushProperty(LogPropertyKeys.EventId, context.Message.EventId))
        using (LogContext.PushProperty(LogPropertyKeys.MessageId, context.MessageId))
        using (LogContext.PushProperty(LogPropertyKeys.OutboxMessageId, outboxMessageId))
        using (LogContext.PushProperty(LogPropertyKeys.CorrelationId, correlationId))
        using (LogContext.PushProperty(LogPropertyKeys.UserId, context.Message.ChangedByUserId))
        {
            var record = new AuditRecord
            {
                Id = Guid.NewGuid(),
                ServiceName = "TaskService",
                EventType = nameof(TaskBlockedIntegrationEventV1),
                EntityId = context.Message.EventId,
                UserId = context.Message.ChangedByUserId,
                Payload = JsonSerializer.Serialize(context.Message),
                OccurredOn = context.Message.OccurredOn,
                CorrelationId = context.CorrelationId?.ToString()
                                ?? string.Empty
            };

            try
            {
                dbContext.AuditRecords.Add(record);
                await dbContext.SaveChangesAsync(context.CancellationToken);

                using (LogContext.PushProperty(LogPropertyKeys.Outcome, LogOutcome.Success))
                    Log.Information("Audit record persisted");
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty(LogPropertyKeys.Outcome, LogOutcome.Retry))
                    Log.Warning(ex, "Audit consume failed, retrying");

                throw;
            }
        }
    }
}
