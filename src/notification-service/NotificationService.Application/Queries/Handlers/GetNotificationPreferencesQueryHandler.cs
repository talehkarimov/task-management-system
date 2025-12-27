using MediatR;
using NotificationService.Application.Dtos;
using NotificationService.Application.Interfaces;

namespace NotificationService.Application.Queries.Handlers;

public sealed class GetNotificationPreferencesQueryHandler(
    INotificationPreferenceProvider provider)
    : IRequestHandler<GetNotificationPreferencesQuery, NotificationPreferencesDto>
{
    public async Task<NotificationPreferencesDto> Handle(
        GetNotificationPreferencesQuery request,
        CancellationToken cancellationToken)
    {
        var prefs = await provider.GetAsync(
            request.UserId,
            cancellationToken);

        return new NotificationPreferencesDto(
            prefs.EmailEnabled,
            prefs.InAppEnabled,
            prefs.Email);
    }
}
