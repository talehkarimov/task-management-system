using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using System.Text.Json;
using Serilog;
using LogContext = Serilog.Context.LogContext;
namespace NotificationService.Application.Services;

public sealed class NotificationDispatcherService(
    INotificationRepository notificationRepository,
    INotificationDeliveryRepository deliveryRepository,
    INotificationPreferenceProvider preferenceProvider,
    IEnumerable<INotificationChannelSender> channelSenders)
    : INotificationDispatcherService
{
    public async Task DispatchAsync(
        NotificationIntent intent,
        CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("RecipientUserId", intent.RecipientUserId))
        using (LogContext.PushProperty("NotificationType", intent.NotificationType))
        {
            var preferences =
                await preferenceProvider.GetAsync(
                    intent.RecipientUserId,
                    cancellationToken);

            var notification = new Notification
            {
                UserId = intent.RecipientUserId,
                Type = intent.NotificationType,
                Payload = JsonSerializer.Serialize(intent.Payload),
                CreatedAt = DateTime.UtcNow,
                Status = NotificationStatus.Unread
            };

            await notificationRepository.AddAsync(
                notification,
                cancellationToken);

            Log.Information(
                "Notification created with Id {NotificationId}",
                notification.Id);

            var enabledChannels = ResolveChannels(preferences);

            Log.Information(
                "Resolved notification channels: {Channels}",
                enabledChannels);

            foreach (var sender in channelSenders
                .Where(s => enabledChannels.Contains(s.Channel)))
            {
                using (LogContext.PushProperty("Channel", sender.Channel))
                {
                    var delivery = new NotificationDelivery
                    {
                        NotificationId = notification.Id,
                        Channel = sender.Channel,
                        Status = DeliveryStatus.Pending,
                        CreatedAt = DateTime.UtcNow
                    };

                    await deliveryRepository.AddAsync(
                        delivery,
                        cancellationToken);

                    try
                    {
                        Log.Information(
                            "Sending notification via channel");

                        await sender.SendAsync(
                            notification,
                            cancellationToken);

                        delivery.MarkSucceeded();

                        Log.Information(
                            "Notification delivered successfully");
                    }
                    catch (Exception ex)
                    {
                        delivery.MarkFailed(ex.Message);

                        Log.Error(
                            ex,
                            "Notification delivery failed");
                    }

                    await deliveryRepository.UpdateAsync(
                        delivery,
                        cancellationToken);
                }
            }
        }
    }

    private IEnumerable<NotificationChannel> ResolveChannels(
        UserNotificationPreference preference)
    {
        var channels = new List<NotificationChannel>
        {
            NotificationChannel.InApp
        };

        if (preference.EmailEnabled)
            channels.Add(NotificationChannel.Email);

        return channels;
    }
}
