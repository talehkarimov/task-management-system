using TaskService.Application.Events.IntegrationEvents;

namespace TaskService.Application.Events.Mapping;

public interface IIntegrationEventMapper
{
    IIntegrationEvent Map(IDomainEvent domainEvent);
}
