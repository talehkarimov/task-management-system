using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Consumers;

namespace NotificationService.Infrastructure.Messaging;

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
                cfg.Host(new Uri(
                            "amqps://fxfusnbm:qhlEeivcfYNni6tk5NFDN8Vsq1Kn18sE@campbell.lmq.cloudamqp.com/fxfusnbm"));
            });
        });

        return services;
    }
}
