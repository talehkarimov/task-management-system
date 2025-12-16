using MediatR;
using TaskService.Application.Common;
using TaskService.Application.Exceptions;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Commands.Handlers;

public class CompleteTaskHandler(ITaskRepository taskRepository, ICacheService cache) : IRequestHandler<CompleteTaskCommand>
{
    public async Task Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null) throw new NotFoundException($"Task with ID '{request.TaskId}' was not found.");

        if (task.Status == Domain.Enums.TaskStatus.Done) throw new BusinessRuleViolationException("Task is already completed.");

        task.Complete();
        await taskRepository.UpdateAsync(task, cancellationToken);
        await cache.RemoveAsync(CacheKeys.TaskById(task.Id), cancellationToken);
    }
}
