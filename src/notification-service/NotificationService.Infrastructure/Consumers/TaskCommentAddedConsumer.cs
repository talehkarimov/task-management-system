using MassTransit;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using Common.Messaging.IntegrationEvents.TaskService;
using NotificationService.Domain.Enums;
using LogContext = Serilog.Context.LogContext;

namespace NotificationService.Infrastructure.Consumers;

public sealed class TaskCommentAddedConsumer(INotificationDispatcherService dispatcherService) : IConsumer<TaskCommentAddedIntegrationEventV1>
{
    public async Task Consume(ConsumeContext<TaskCommentAddedIntegrationEventV1> context)
    {
        using (LogContext.PushProperty("IntegrationEvent", nameof(TaskCommentAddedIntegrationEventV1)))
        using (LogContext.PushProperty("EventId", context.Message.EventId))
        using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
        using (LogContext.PushProperty("UserId", context.Message.CommentedByUserId))
        {
            Serilog.Log.Information("TaskCommentAddedIntegrationEvent received");
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
}
