using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Models;
using NotificationService.Infrastructure.Persistence;

namespace NotificationService.Infrastructure.Repositories;

public sealed class UserNotificationPreferenceProvider(NotificationDbContext context)
    : INotificationPreferenceProvider
{
    public async Task<UserNotificationPreference> GetAsync(
        Guid userId,
        CancellationToken ct)
    {
        var preference = await context.UserNotificationPreferences
            .AsNoTracking()
            .Where(p => p.UserId == userId)
            .FirstOrDefaultAsync(ct);

        return preference ??
               throw new InvalidOperationException(
                   $"Notification preferences not found for user {userId}");
    }
}
