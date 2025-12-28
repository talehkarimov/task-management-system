using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;
using LogContext = Serilog.Context.LogContext;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskStatusChangedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskStatusChangedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskStatusChangedIntegrationEventV1> context) 
    {
        using (LogContext.PushProperty("IntegrationEvent", nameof(TaskStatusChangedIntegrationEventV1)))
        using (LogContext.PushProperty("EventId", context.Message.EventId))
        using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
        using (LogContext.PushProperty("UserId", context.Message.ChangedByUserId))
        {
            Serilog.Log.Information("TaskStatusChangedIntegrationEvent received");
            var intent = new NotificationIntent
            {
                RecipientUserId = context.Message.ChangedByUserId,
                NotificationType = NotificationType.TaskStatusChanged,
                Payload = new Dictionary<string, string>
                {
                    ["TaskId"] = context.Message.TaskId.ToString(),
                    ["NewStatus"] = context.Message.NewStatus.ToString()
                }
            };

            await dispatcherService.DispatchAsync(intent, context.CancellationToken);
        }
    }
}
