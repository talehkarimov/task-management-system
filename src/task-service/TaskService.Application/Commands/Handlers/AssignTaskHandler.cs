using MediatR;
using TaskService.Application.Interfaces;

namespace TaskService.Application.Commands.Handlers;

public class AssignTaskHandler(ITaskRepository taskRepository) : IRequestHandler<AssignTaskCommand>
{
    public async Task Handle(AssignTaskCommand request, CancellationToken cancellationToken)
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
                "Completed task cannot be assigned.");
        }

        task.Assign(request.AssigneeUserId);
        await taskRepository.UpdateAsync(task, cancellationToken);
    }
}
