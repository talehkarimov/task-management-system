using MediatR;
using NotificationService.Application.Dtos;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Queries.Handlers;

public sealed class GetMyNotificationsQueryHandler(
    INotificationRepository repository)
    : IRequestHandler<GetMyNotificationsQuery, IReadOnlyCollection<NotificationDto>>
{
    public async Task<IReadOnlyCollection<NotificationDto>> Handle(
        GetMyNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var notifications =
            await repository.GetByUserAsync(
                request.UserId,
                cancellationToken);

        return notifications
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new NotificationDto(
                x.Id,
                x.Type,
                x.Payload,
                x.CreatedAt,
                x.Status))
            .ToList();
    }
}