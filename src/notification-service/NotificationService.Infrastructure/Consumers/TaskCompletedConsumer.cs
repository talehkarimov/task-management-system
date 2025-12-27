using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using NotificationService.Contracts.IntegrationEvents;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskCompletedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskCompletedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskCompletedIntegrationEventV1> context)
    {
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
