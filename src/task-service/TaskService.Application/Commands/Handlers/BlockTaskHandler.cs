using MediatR;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Commands.Handlers;

public class BlockTaskHandler(ITaskRepository taskRepository) : IRequestHandler<BlockTaskCommand>
{
    public async Task Handle(BlockTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetByIdAsync(request.TaskId, cancellationToken);
        if (task is null)
        {
            throw new InvalidOperationException(
                $"Task with ID '{request.TaskId}' was not found.");
        }

        if (task.Status == Domain.Enums.TaskStatus.Done)
        {
            throw new InvalidOperationException(
                "Completed task cannot be blocked.");
        }

        task.Block();
        await taskRepository.UpdateAsync(task, cancellationToken);
    }
}
