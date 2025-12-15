using MediatR;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Commands.Handlers;

public class ChangeTaskStatusHandler(ITaskRepository taskRepository) : IRequestHandler<ChangeTaskStatusCommand>
{
    public async Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
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
                "Completed task cannot be changed.");
        }

        task.ChangeStatus(request.NewStatus);
        await taskRepository.UpdateAsync(task, cancellationToken);
    }
}
