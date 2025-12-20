using MediatR;
using TaskService.Application.Common;
using TaskService.Application.Events;
using TaskService.Application.Exceptions;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Commands.Handlers;

public class ChangeTaskStatusHandler(ITaskRepository taskRepository, ICacheService cache, IDomainEventDispatcher eventDispatcher) : IRequestHandler<ChangeTaskStatusCommand>
{
    public async Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null) throw new NotFoundException($"Task with ID '{request.TaskId}' was not found.");

        if (task.Status == Domain.Enums.TaskStatus.Done) throw new BusinessRuleViolationException("Completed task cannot be changed.");

        task.ChangeStatus(request.NewStatus);
        await taskRepository.UpdateAsync(task, cancellationToken);

        await eventDispatcher.DispatchAsync(
            new TaskStatusChangedEvent(task.Id, task.Status, request.ChangedByUserId),
            cancellationToken
        );

        await cache.RemoveAsync(CacheKeys.TaskById(task.Id), cancellationToken);
    }
}
