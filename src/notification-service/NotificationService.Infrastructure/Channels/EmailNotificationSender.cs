using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Email;
using System.Text.Json;

namespace NotificationService.Infrastructure.Channels;

public sealed class EmailNotificationSender(IEmailSender emailSender,
    EmailTemplateResolver templateResolver,
    INotificationPreferenceProvider preferenceProvider) : INotificationChannelSender
{
    public NotificationChannel Channel => NotificationChannel.Email;

    public async Task SendAsync(
        Notification notification,
        CancellationToken cancellationToken)
    {
        var preference =
            await preferenceProvider.GetAsync(
                notification.UserId,
                cancellationToken);

        var payload =
            JsonSerializer.Deserialize<Dictionary<string, string>>(
                notification.Payload)!;

        var (subject, body) =
            templateResolver.Resolve(notification.Type, payload);

        await emailSender.SendAsync(
            preference.Email,
            subject,
            body,
            cancellationToken);
    }
}
