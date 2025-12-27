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

    public Task SaveAsync(Guid userId, string email, bool emailEnabled, bool inAppEnabled)
    {
        var existingPreference = context.UserNotificationPreferences
            .FirstOrDefault(p => p.UserId == userId);
        if (existingPreference is not null)
        {
            existingPreference.Email = email;
            existingPreference.EmailEnabled = emailEnabled;
            existingPreference.InAppEnabled = inAppEnabled;
        }
        else
        {
            var preference = new UserNotificationPreference
            {
                UserId = userId,
                Email = email,
                EmailEnabled = emailEnabled,
                InAppEnabled = inAppEnabled
            };
            context.UserNotificationPreferences.Add(preference);
        }
        
        return context.SaveChangesAsync();
    }
}
