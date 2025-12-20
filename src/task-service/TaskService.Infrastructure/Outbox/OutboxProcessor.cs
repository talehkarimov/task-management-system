using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Outbox;

public sealed class OutboxProcessor(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
            var bus = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedAt == null)
                .OrderBy(m => m.CreatedAt)
                .Take(20)
                .ToListAsync(stoppingToken);

            if(messages.Count == 0)
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            foreach (var message in messages)
            {
                var eventType = Type.GetType(message.Type);
                if(eventType is null) continue;

                var domainEvent = JsonSerializer.Deserialize(message.Payload, eventType);

                await bus.Publish(domainEvent!, stoppingToken);

                message.ProcessedAt = DateTime.Now;
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
