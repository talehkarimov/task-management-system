using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;
using LogContext = Serilog.Context.LogContext;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskCompletedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskCompletedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskCompletedIntegrationEventV1> context)
    {
        using (LogContext.PushProperty("IntegrationEvent", nameof(TaskCompletedIntegrationEventV1)))
        using (LogContext.PushProperty("EventId", context.Message.EventId))
        using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
        using (LogContext.PushProperty("UserId", context.Message.CompletedByUserId))
        {
            Serilog.Log.Information("TaskCompletedIntegrationEvent received");
            var intent = new NotificationIntent
            {
                RecipientUserId = context.Message.CompletedByUserId,
                NotificationType = NotificationType.TaskCompleted,
                Payload = new Dictionary<string, string>
                {
                    ["TaskId"] = context.Message.TaskId.ToString()
                }
            };

            await dispatcherService.DispatchAsync(intent, context.CancellationToken);
        }
    }
}
