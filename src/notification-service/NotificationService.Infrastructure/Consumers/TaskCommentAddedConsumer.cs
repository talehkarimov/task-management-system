using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using NotificationService.Contracts.IntegrationEvents;
using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskCommentAddedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskCommentAddedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskCommentAddedIntegrationEventV1> context)
    {
        var intent = new NotificationIntent
        {
            RecipientUserId = context.Message.CommentedByUserId,
            NotificationType = NotificationType.TaskCommentAdded,
            Payload = new Dictionary<string, string>
            {
                ["TaskId"] = context.Message.TaskId.ToString()
            }
        };

        await dispatcherService.DispatchAsync(intent, context.CancellationToken);
    }
}
