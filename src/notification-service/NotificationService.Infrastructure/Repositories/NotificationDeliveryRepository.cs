using NotificationService.Application.Interfaces;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Infrastructure.Repositories;

public sealed class NotificationDeliveryRepository(NotificationDbContext context)
    : INotificationDeliveryRepository
{
    public async Task AddAsync(NotificationDelivery delivery, CancellationToken ct)
    {
        context.NotificationDeliveries.Add(delivery);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(NotificationDelivery delivery, CancellationToken ct)
    {
        context.NotificationDeliveries.Update(delivery);
        await context.SaveChangesAsync(ct);
    }
}
