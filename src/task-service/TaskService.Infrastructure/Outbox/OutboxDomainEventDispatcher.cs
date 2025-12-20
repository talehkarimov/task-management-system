using System.Text.Json;
using TaskService.Application.Events;
using TaskService.Application.Interfaces;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Outbox;

public sealed class OutboxDomainEventDispatcher(TaskDbContext dbContext) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = domainEvent.GetType().AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(domainEvent),
            CreatedAt = DateTime.Now
        };
        dbContext.OutboxMessages.Add(outboxMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
