using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;
using LogContext = Serilog.Context.LogContext;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskAssignedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskAssignedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskAssignedIntegrationEventV1> context)
    {
        using (LogContext.PushProperty("IntegrationEvent", nameof(TaskAssignedIntegrationEventV1)))
        using (LogContext.PushProperty("EventId", context.Message.EventId))
        using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
        using (LogContext.PushProperty("UserId", context.Message.ChangedByUserId))
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
            await dispatcherService.DispatchAsync(intent, context.CancellationToken);
        }
    }

}
