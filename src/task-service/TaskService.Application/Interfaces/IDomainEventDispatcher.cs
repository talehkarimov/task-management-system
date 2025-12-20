using TaskService.Application.Events;

namespace TaskService.Application.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent,CancellationToken cancellationToken);
}
