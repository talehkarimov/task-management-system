using MediatR;
using NotificationService.Application.Dtos;

namespace NotificationService.Application.Queries;

public sealed record GetMyNotificationsQuery(Guid UserId)
    : IRequest<IReadOnlyCollection<NotificationDto>>;
