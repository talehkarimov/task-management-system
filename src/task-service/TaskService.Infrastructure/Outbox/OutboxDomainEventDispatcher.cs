using System.Text.Json;
using TaskService.Application.Events;
using TaskService.Application.Events.Mapping;
using TaskService.Application.Interfaces;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Outbox;

public sealed class OutboxDomainEventDispatcher(TaskDbContext dbContext, IRequestContext context, IIntegrationEventMapper mapper) : IDomainEventDispatcher
{
    public async Task DispatchAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var integrationEvent = mapper.Map(domainEvent);
        var outboxMessage = new OutboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().Name,
            Payload = JsonSerializer.Serialize(
                integrationEvent,
                integrationEvent.GetType()),
            CreatedAt = DateTime.UtcNow,
            CorrelationId = context.CorrelationId,
            UserId = context.UserId,
            OrganizationId = context.OrganizationId,
            AttemptCount = 0
        };
        dbContext.OutboxMessages.Add(outboxMessage);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
