namespace TaskService.Application.Events;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
