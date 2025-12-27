namespace TaskService.Application.Events.IntegrationEvents;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}