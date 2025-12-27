using NotificationService.Application.Interfaces;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Infrastructure.Repositories;

public sealed class NotificationRepository(NotificationDbContext context)
    : INotificationRepository
{
    public async Task AddAsync(Notification notification, CancellationToken ct)
    {
        context.Notifications.Add(notification);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Notification notification, CancellationToken ct)
    {
        context.Notifications.Update(notification);
        await context.SaveChangesAsync(ct);
    }
}
