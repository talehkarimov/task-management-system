using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using NotificationService.Contracts.IntegrationEvents;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskStatusChangedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskStatusChangedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskStatusChangedIntegrationEventV1> context) 
    {
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
