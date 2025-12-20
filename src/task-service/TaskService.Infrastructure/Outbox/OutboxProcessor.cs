using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using TaskService.Application.Common;
using TaskService.Infrastructure.Persistence;
using LogContext = Serilog.Context.LogContext;

namespace TaskService.Infrastructure.Outbox;

public sealed class OutboxProcessor(IServiceProvider serviceProvider) : BackgroundService
{
    private const int BatchSize = 20;
    private const int DelayMs = 1000;
    private const int StuckAttemptThreshold = 10;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
            var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);

            if (messages.Count == 0)
            {
                await Task.Delay(DelayMs, stoppingToken);
                continue;
            }

            foreach (var message in messages)
            {
                using (LogContext.PushProperty(LogKeys.OutboxMessageId, message.Id))
                using (LogContext.PushProperty(LogKeys.EventType, message.Type))
                using (LogContext.PushProperty(LogKeys.CorrelationId, message.CorrelationId))
                using (LogContext.PushProperty(LogKeys.UserId, message.UserId))
                using (LogContext.PushProperty(LogKeys.OrganizationId, message.OrganizationId))
                using (LogContext.PushProperty(LogKeys.OutboxAttempt, message.AttemptCount))
                {
                    if (message.AttemptCount >= StuckAttemptThreshold)
                    {
                        Serilog.Log.Warning("Outbox message is potentially stuck (attempt threshold reached)");
                    }

                    Type? eventType = null;
                    object? domainEvent = null;
                    try
                    {
                        eventType = Type.GetType(message.Type);
                        if (eventType is null)
                        {
                            message.AttemptCount++;
                            message.LastError = $"Type resolution failed: {message.Type}";
                            Serilog.Log.Warning("Outbox publish skipped: event type not found");
                            continue;
                        }

                        domainEvent = JsonSerializer.Deserialize(message.Payload, eventType);
                        if (domainEvent is null)
                        {
                            message.AttemptCount++;
                            message.LastError = $"Deserialization returned null for type: {message.Type}";
                            Serilog.Log.Warning("Outbox publish skipped: deserialization produced null");
                            continue;
                        }

                        await bus.Publish(domainEvent, publishContext =>
                        {
                            if (!string.IsNullOrWhiteSpace(message.CorrelationId))
                                publishContext.Headers.Set("X-Correlation-Id", message.CorrelationId);

                            if (message.UserId.HasValue)
                                publishContext.Headers.Set("X-User-Id", message.UserId.Value.ToString());

                            if (message.OrganizationId.HasValue)
                                publishContext.Headers.Set("X-Org-Id", message.OrganizationId.Value.ToString());

                            publishContext.Headers.Set("X-Outbox-Message-Id", message.Id.ToString());
                        }, stoppingToken);

                        message.ProcessedAt = DateTime.Now;
                        message.LastError = null;

                        Serilog.Log.Information("Outbox message published successfully");
                    }
                    catch (Exception ex)
                    {
                        message.AttemptCount++;
                        message.LastError = ex.Message;

                        Serilog.Log.Error(ex, "Outbox message publish failed");
                    }
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(DelayMs, stoppingToken);
        }
    }
}
