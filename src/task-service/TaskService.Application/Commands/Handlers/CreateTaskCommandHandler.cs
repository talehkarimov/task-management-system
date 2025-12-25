using MediatR;
using TaskService.Application.Events;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entitites;

namespace TaskService.Application.Commands.Handlers;

public class CreateTaskCommandHandler(ITaskRepository taskRepository, IDomainEventDispatcher eventDispatcher) : IRequestHandler<CreateTaskCommand, Guid>
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

        await eventDispatcher.DispatchAsync(
            new TaskCreatedEventV1(task.ProjectId, 
            task.Title, 
            task.Description,
            task.ReporterUserId,
            task.AssigneeUserId,
            task.DueDate,
            task.Priority), 
            cancellationToken
        );

        return task.Id;
    }
}
