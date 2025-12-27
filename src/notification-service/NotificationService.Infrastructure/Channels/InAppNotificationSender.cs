using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;

namespace NotificationService.Infrastructure.Channels;

public sealed class InAppNotificationSender : INotificationChannelSender
{
    public NotificationChannel Channel => NotificationChannel.InApp;

    public Task SendAsync(Notification notification, CancellationToken ct) => Task.CompletedTask;
}
