using MediatR;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Commands.Handlers;

public sealed class UpdateNotificationPreferencesCommandHandler(
    INotificationPreferenceProvider provider)
    : IRequestHandler<UpdateNotificationPreferencesCommand>
{
    public async Task Handle(
        UpdateNotificationPreferencesCommand request,
        CancellationToken cancellationToken)
    {
        await provider.SaveAsync(
            request.UserId,
            request.Email,
            request.EmailEnabled,
            request.InAppEnabled);
    }
}