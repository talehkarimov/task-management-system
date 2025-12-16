using MediatR;
using TaskService.Application.Common;
using TaskService.Application.Exceptions;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Commands.Handlers;

public class BlockTaskHandler(ITaskRepository taskRepository, ICacheService cache) : IRequestHandler<BlockTaskCommand>
{
    public async Task Handle(BlockTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null) throw new NotFoundException($"Task with ID '{request.TaskId}' was not found.");

        if (task.Status == Domain.Enums.TaskStatus.Done) throw new BusinessRuleViolationException("Task is already completed.");
        task.Block();
        await taskRepository.UpdateAsync(task, cancellationToken);
        await cache.RemoveAsync(CacheKeys.TaskById(task.Id), cancellationToken);
    }
}
