using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Infrastructure.Repositories;

public sealed class NotificationRepository(NotificationDbContext context)
    : INotificationRepository
{
    public async Task<List<Notification>> GetByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await context.Notifications
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task AddAsync(Notification notification, CancellationToken ct)
    {
        context.Notifications.Add(notification);
        await context.SaveChangesAsync(ct);
    }

    public async Task MarkAsReadAsync(Guid notificationId, CancellationToken cancellationToken)
    {
        var notification  = await context.Notifications
           .AsNoTracking()
           .Where(x => x.Id == notificationId)
           .FirstOrDefaultAsync();

        if(notification is null)
            throw new InvalidOperationException("Notification not found.");

        notification.Status = NotificationStatus.Read;
        notification.ReadAt = DateTime.Now;
        await context.SaveChangesAsync(cancellationToken);
    }
}
