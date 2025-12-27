using MediatR;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Commands.Handlers;

public sealed class MarkNotificationAsReadCommandHandler(
    INotificationRepository repository)
    : IRequestHandler<MarkNotificationAsReadCommand>
{
    public async Task Handle(
        MarkNotificationAsReadCommand request,
        CancellationToken cancellationToken)
    {
        await repository.MarkAsReadAsync(request.NotificationId, cancellationToken);
    }
}
