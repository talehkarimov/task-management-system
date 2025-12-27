using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskBlockedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskBlockedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskBlockedIntegrationEventV1> context)
    {
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
