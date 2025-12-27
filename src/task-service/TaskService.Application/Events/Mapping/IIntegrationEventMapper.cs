using Common.Messaging.IntegrationEvents.TaskService;

namespace TaskService.Application.Events.Mapping;

public interface IIntegrationEventMapper
{
    IIntegrationEvent Map(IDomainEvent domainEvent);
}
