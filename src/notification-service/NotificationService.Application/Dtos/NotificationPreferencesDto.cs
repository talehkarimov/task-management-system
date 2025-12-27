namespace NotificationService.Application.Dtos;
public sealed record NotificationPreferencesDto(
    bool EmailEnabled,
    bool InAppEnabled,
    string Email);
