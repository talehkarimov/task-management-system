using NotificationService.Domain.Enums;

namespace NotificationService.Application.Models;

public sealed record NotificationIntent
{
    public Guid RecipientUserId { get; init; }
    public NotificationType NotificationType { get; init; }
    public IReadOnlyDictionary<string, string> Payload { get; init; } = null!;
}