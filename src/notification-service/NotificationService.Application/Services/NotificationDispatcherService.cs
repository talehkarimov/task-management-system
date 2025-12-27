using NotificationService.Application.Interfaces;
using NotificationService.Application.Models;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using System.Text.Json;

namespace NotificationService.Application.Services;

public sealed class NotificationDispatcherService(INotificationRepository notificationRepository,
        INotificationDeliveryRepository deliveryRepository,
        INotificationPreferenceProvider preferenceProvider,
        IEnumerable<INotificationChannelSender> channelSenders) : INotificationDispatcherService
{
    public async Task DispatchAsync(NotificationIntent intent, CancellationToken cancellationToken)
    {
        var preferences = await preferenceProvider.GetAsync(intent.RecipientUserId, cancellationToken);
        var notification = new Notification
        {
            UserId = intent.RecipientUserId,
            Type =  intent.NotificationType,
            Payload =  JsonSerializer.Serialize(intent.Payload),
            CreatedAt = DateTime.Now
        };
        await notificationRepository.AddAsync(notification, cancellationToken);

        var enabledChannels = ResolveChannels(preferences);

        foreach (var sender in channelSenders.Where(s => enabledChannels.Contains(s.Channel)))
        {
            var delivery = new NotificationDelivery
            {
                NotificationId = notification.Id,
                Channel = sender.Channel,
                Status = DeliveryStatus.Pending,
                CreatedAt = DateTime.Now
            };

            await deliveryRepository.AddAsync(delivery, cancellationToken);

            try
            {
                await sender.SendAsync(notification, cancellationToken);
                delivery.MarkSucceeded();
            }
            catch (Exception ex)
            {
                delivery.MarkFailed(ex.Message);
            }

            await deliveryRepository.UpdateAsync(delivery, cancellationToken);
        }
    }

    private IEnumerable<NotificationChannel> ResolveChannels(UserNotificationPreference preference)
    {
        var chanels = new List<NotificationChannel>()
        {
            NotificationChannel.InApp
        };

        if(preference.EmailEnabled)
            chanels.Add(NotificationChannel.Email);
        return chanels;
    }
}
