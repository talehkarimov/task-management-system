using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Interfaces;
using NotificationService.Application.Services;

namespace NotificationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
       this IServiceCollection services)
    {
        services.AddScoped<INotificationDispatcherService, NotificationDispatcherService>();
        return services;
    }
}
