using System.Text.Json;
using TaskService.Application.Events;
using TaskService.Application.Interfaces;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Outbox;

public sealed class OutboxDomainEventDispatcher(TaskDbContext dbContext, IRequestContext context) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = domainEvent.GetType().AssemblyQualifiedName!,
            Payload = JsonSerializer.Serialize(domainEvent),
            CreatedAt = DateTime.Now,
            CorrelationId = context.CorrelationId,
            UserId = context.UserId,
            OrganizationId = context.OrganizationId,
            AttemptCount = 0
        };
        dbContext.OutboxMessages.Add(outboxMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
