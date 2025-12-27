using MediatR;

namespace NotificationService.Application.Commands;

public sealed record UpdateNotificationPreferencesCommand(
    Guid UserId,
    bool EmailEnabled,
    bool InAppEnabled,
    string Email) : IRequest;
