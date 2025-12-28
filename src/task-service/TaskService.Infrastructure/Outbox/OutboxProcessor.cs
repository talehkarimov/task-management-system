using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using Common.Logging.Observability;
using TaskService.Application.Common;
using TaskService.Infrastructure.Persistence;
using LogContext = Serilog.Context.LogContext;

namespace TaskService.Infrastructure.Outbox;

public sealed class OutboxProcessor(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
            var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedAt == null && m.AttemptCount < OutboxPolicy.MaxAttempts)
                .OrderBy(m => m.CreatedAt)
                .Take(OutboxPolicy.BatchSize)
                .ToListAsync(stoppingToken);

            if (messages.Count == 0)
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            foreach (var message in messages)
            {
                var operationName = "PublishIntegrationEvent";

                using (LogContext.PushProperty(LogKeys.Component, "Outbox"))
                using (LogContext.PushProperty(LogKeys.OperationName, operationName))
                using (LogContext.PushProperty(LogKeys.OutboxMessageId, message.Id))
                using (LogContext.PushProperty(LogKeys.EventType, message.Type))
                using (LogContext.PushProperty(LogKeys.CorrelationId, message.CorrelationId))
                using (LogContext.PushProperty(LogKeys.UserId, message.UserId))
                using (LogContext.PushProperty(LogKeys.OrganizationId, message.OrganizationId))
                using (LogContext.PushProperty(LogKeys.OutboxAttempt, message.AttemptCount))
                {
                    Type? eventType = null;
                    object? domainEvent = null;
                    try
                    {
                        eventType = Type.GetType(message.Type);
                        if (eventType is null)
                        {
                            Poison(message, $"Event type not found: {message.Type}");
                            continue;
                        }

                        domainEvent = JsonSerializer.Deserialize(message.Payload, eventType);
                        if (domainEvent is null)
                        {
                            Poison(message, "Deserialization returned null");
                            continue;
                        }

                        await bus.Publish(domainEvent, publishContext =>
                        {
                            publishContext.Headers.Set(HeaderNames.OutboxMessageId, message.Id.ToString());
                            publishContext.Headers.Set(HeaderNames.CorrelationId, message.CorrelationId);

                            if (message.UserId.HasValue)
                                publishContext.Headers.Set(HeaderNames.UserId, message.UserId.Value.ToString());

                            if (message.OrganizationId.HasValue)
                                publishContext.Headers.Set(HeaderNames.OrganizationId, message.OrganizationId.Value.ToString());

                        }, stoppingToken);

                        message.ProcessedAt = DateTime.Now;
                        message.LastError = null;

                        using (LogContext.PushProperty(LogKeys.Outcome, LogOutcome.Success))
                        {
                            Serilog.Log.Information("Outbox message published successfully");
                        }
                    }
                    catch (Exception ex)
                    {
                        message.AttemptCount++;
                        message.LastError = ex.Message;

                        using (LogContext.PushProperty(LogKeys.Outcome, LogOutcome.Failure))
                        {
                            Serilog.Log.Error(ex, "Outbox message publish failed");
                        }

                        await Task.Delay(
                           OutboxPolicy.FailureBackoffMs,
                           stoppingToken);
                    }
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }

    private static void Poison(OutboxMessage message, string reason)
    {
        message.AttemptCount = OutboxPolicy.MaxAttempts;
        message.LastError = reason;

        using (LogContext.PushProperty(LogKeys.Component, "Outbox"))
        using (LogContext.PushProperty(LogKeys.OperationName, "PublishIntegrationEvent"))
        using (LogContext.PushProperty(LogKeys.Outcome, LogOutcome.Poisoned))
        using (LogContext.PushProperty(LogKeys.OutboxMessageId, message.Id))
        using (LogContext.PushProperty(LogKeys.EventType, message.Type))
        using (LogContext.PushProperty(LogKeys.CorrelationId, message.CorrelationId))
        {
            Serilog.Log.Fatal(
                "Outbox message poisoned and will no longer be retried");
        }
    }
}
