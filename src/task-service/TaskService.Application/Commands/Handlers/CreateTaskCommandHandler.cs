using MediatR;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entitites;

namespace TaskService.Application.Commands.Handlers;

public class CreateTaskCommandHandler(ITaskRepository taskRepository) : IRequestHandler<CreateTaskCommand, Guid>
{
    public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = new TaskItem(
            request.ProjectId,
            request.Title,
            request.Description,
            request.ReporterUserId,
            request.AssigneeUserId,
            request.DueDate,
            request.Priority);

        await taskRepository.AddAsync(task, cancellationToken);

        return task.Id;
    }
}
