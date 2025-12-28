using MassTransit.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace TaskService.API.Health;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddServiceHealthChecks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHealthChecks()

            .AddCheck(
                "self",
                () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy(),
                tags: new[] { HealthCheckTags.Live })

            .AddSqlServer(
                configuration.GetConnectionString("DefaultConnection")!,
                name: "sqlserver",
                tags: new[] { HealthCheckTags.Ready, HealthCheckTags.Database });

        return services;
    }
}
