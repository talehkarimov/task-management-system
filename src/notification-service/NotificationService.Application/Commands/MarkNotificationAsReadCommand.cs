using MediatR;

namespace NotificationService.Application.Commands;

public sealed record MarkNotificationAsReadCommand(
    Guid NotificationId,
    Guid UserId) : IRequest;
