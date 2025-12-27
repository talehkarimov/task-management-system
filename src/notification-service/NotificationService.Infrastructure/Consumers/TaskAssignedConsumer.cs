using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskAssignedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskAssignedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskAssignedIntegrationEventV1> context)
    {
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
