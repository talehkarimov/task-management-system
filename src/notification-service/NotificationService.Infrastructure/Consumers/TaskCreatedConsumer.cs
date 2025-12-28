using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;
using Common.Logging.Observability;
using LogContext = Serilog.Context.LogContext;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskCreatedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskCreatedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskCreatedIntegrationEventV1> context)
    {
		var correlationId = ConsumerObservability.ResolveCorrelationId(context);
        var operationName = $"Consume:{nameof(TaskCreatedIntegrationEventV1)}";
		var outboxMessageId = ConsumerObservability.ResolveOutboxMessageId(context);

        using (LogContext.PushProperty(LogPropertyKeys.Component, "Consumer"))
        using (LogContext.PushProperty(LogPropertyKeys.OperationName, operationName))
        using (LogContext.PushProperty(LogPropertyKeys.EventType, nameof(TaskCreatedIntegrationEventV1)))
        using (LogContext.PushProperty(LogPropertyKeys.EventId, context.Message.EventId))
		using (LogContext.PushProperty(LogPropertyKeys.MessageId, context.MessageId))
		using (LogContext.PushProperty(LogPropertyKeys.OutboxMessageId, outboxMessageId))
        using (LogContext.PushProperty(LogPropertyKeys.CorrelationId, correlationId))
        using (LogContext.PushProperty(LogPropertyKeys.UserId, context.Message.ReporterUserId))
        {
            Serilog.Log.Information("TaskCreatedIntegrationEvent received");

            var intent = new NotificationIntent
            {
                RecipientUserId = context.Message.ReporterUserId,
                NotificationType = NotificationType.TaskCreated,
                Payload = new Dictionary<string, string>
                {
                    ["ProjectId"] = context.Message.ProjectId.ToString(),
                    ["Priority"] = context.Message.Priority.ToString()
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
