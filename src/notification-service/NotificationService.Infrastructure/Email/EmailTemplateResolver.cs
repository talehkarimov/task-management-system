using NotificationService.Domain.Enums;

namespace NotificationService.Infrastructure.Email;

public sealed class EmailTemplateResolver
{
    public (string Subject, string Body) Resolve(
        NotificationType type,
        IReadOnlyDictionary<string, string> payload)
    {
        return type switch
        {
            NotificationType.TaskAssigned =>
                ("Task Assigned",
                 $"You have been assigned to task {payload["TaskId"]}"),

            NotificationType.TaskCompleted =>
                ("Task Completed",
                 $"Task {payload["TaskId"]} has been completed"),

            NotificationType.TaskBlocked =>
                ("Task Blocked",
                 $"Task {payload["TaskId"]} was blocked: {payload["Reason"]}"),

            NotificationType.TaskCommentAdded =>
                ("New Comment",
                 $"New comment added to task {payload["TaskId"]}"),

            NotificationType.TaskStatusChanged =>
                ("Task Status Changed",
                 $"Task {payload["TaskId"]} status changed"),

            _ =>
                ("Notification",
                 "You have a new notification")
        };
    }
}
