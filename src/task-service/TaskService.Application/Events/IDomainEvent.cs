namespace TaskService.Application.Events;

public interface IDomainEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
