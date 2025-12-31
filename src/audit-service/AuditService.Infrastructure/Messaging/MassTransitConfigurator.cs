using AuditService.Infrastructure.Consumers;
using Common.Messaging;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditService.Infrastructure.Messaging;

public static class MassTransitConfigurator
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<TaskCreatedConsumer>();
            cfg.AddConsumer<TaskAssignedConsumer>();
            cfg.AddConsumer<TaskStatusChangedConsumer>();
            cfg.AddConsumer<TaskCompletedConsumer>();
            cfg.AddConsumer<TaskBlockedConsumer>();
            cfg.AddConsumer<TaskCommentAddedConsumer>();

            cfg.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMQ:Host"]!));
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
