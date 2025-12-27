namespace Common.Messaging.IntegrationEvents.TaskService;

public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredOn { get; }
}
