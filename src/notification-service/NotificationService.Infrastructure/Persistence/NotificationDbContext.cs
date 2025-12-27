using MassTransit;
using Microsoft.EntityFrameworkCore;
using NotificationService.Domain.Models;

namespace NotificationService.Infrastructure.Persistence;

public sealed class NotificationDbContext(DbContextOptions<NotificationDbContext> options)
    : DbContext(options)
{
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationDelivery> NotificationDeliveries { get; set; }
    public DbSet<UserNotificationPreference> UserNotificationPreferences { get; set; }
}
