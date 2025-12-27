using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskCreatedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskCreatedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskCreatedIntegrationEventV1> context)
    {
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

        await dispatcherService.DispatchAsync(intent, context.CancellationToken);
    }
}
