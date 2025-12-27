using MediatR;
using NotificationService.Application.Dtos;

namespace NotificationService.Application.Queries;

public sealed record GetNotificationPreferencesQuery(Guid UserId)
    : IRequest<NotificationPreferencesDto>;
