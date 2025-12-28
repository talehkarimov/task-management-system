using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;
using LogContext = Serilog.Context.LogContext;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskBlockedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskBlockedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskBlockedIntegrationEventV1> context)
    {
        using (LogContext.PushProperty("IntegrationEvent", nameof(TaskBlockedIntegrationEventV1)))
        using (LogContext.PushProperty("EventId", context.Message.EventId))
        using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
        using (LogContext.PushProperty("UserId", context.Message.ChangedByUserId))
        {
            Serilog.Log.Information("TaskBlockedIntegrationEvent received");

            var intent = new NotificationIntent
            {
                RecipientUserId = context.Message.ChangedByUserId,
                NotificationType = NotificationType.TaskBlocked,
                Payload = new Dictionary<string, string>
                {
                    ["TaskId"] = context.Message.TaskId.ToString(),
                    ["Reason"] = context.Message.Reason
                }
            };

            await dispatcherService.DispatchAsync(intent, context.CancellationToken);
        }
    }
}
