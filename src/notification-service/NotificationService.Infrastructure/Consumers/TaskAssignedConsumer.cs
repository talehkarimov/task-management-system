using Common.Logging.Observability;
using Common.Messaging;
using Common.Messaging.IntegrationEvents.TaskService;
using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using NotificationService.Domain.Enums;
using LogContext = Serilog.Context.LogContext;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskAssignedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskAssignedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskAssignedIntegrationEventV1> context)
    {
        var correlationId = ConsumerObservability.ResolveCorrelationId(context);
        var outboxMessageId = ConsumerObservability.ResolveOutboxMessageId(context);
        var operationName = $"Consume:{nameof(TaskAssignedIntegrationEventV1)}";

        using (LogContext.PushProperty(LogPropertyKeys.Component, "Consumer"))
        using (LogContext.PushProperty(LogPropertyKeys.OperationName, operationName))
        using (LogContext.PushProperty(LogPropertyKeys.EventType, nameof(TaskAssignedIntegrationEventV1)))
        using (LogContext.PushProperty(LogPropertyKeys.EventId, context.Message.EventId))
        using (LogContext.PushProperty(LogPropertyKeys.MessageId, context.MessageId))
        using (LogContext.PushProperty(LogPropertyKeys.OutboxMessageId, outboxMessageId))
        using (LogContext.PushProperty(LogPropertyKeys.CorrelationId, correlationId))
        using (LogContext.PushProperty(LogPropertyKeys.UserId, context.Message.ChangedByUserId))
        {
            Serilog.Log.Information("TaskAssignedIntegrationEvent received");

            var intent = new NotificationIntent
            {
                RecipientUserId = context.Message.AssigneeUserId,
                NotificationType = NotificationType.TaskAssigned,
                Payload = new Dictionary<string, string>
                {
                    { "TaskId", context.Message.TaskId.ToString() },
                    { "ChangedByUserId", context.Message.ChangedByUserId.ToString() }
                }
            };

            try
            {
                await dispatcherService.DispatchAsync(intent, context.CancellationToken);

                using (LogContext.PushProperty(LogPropertyKeys.Outcome, LogOutcome.Success))
                {
                    Serilog.Log.Information("Integration event processed");
                }
            }
            catch (Exception ex)
            {
                using (LogContext.PushProperty(LogPropertyKeys.Outcome, LogOutcome.Failure))
                {
                    Serilog.Log.Error(ex, "Integration event processing failed");
                }
                throw;
            }
        }
    }

}
