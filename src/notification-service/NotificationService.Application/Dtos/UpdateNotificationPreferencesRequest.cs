namespace NotificationService.Application.Dtos;

public sealed record UpdateNotificationPreferencesRequest(
    bool EmailEnabled,
    bool InAppEnabled,
    string Email);
