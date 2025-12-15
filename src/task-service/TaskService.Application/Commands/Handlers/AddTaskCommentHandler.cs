using MediatR;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entitites;

namespace TaskService.Application.Commands.Handlers;

public class AddTaskCommentHandler(ITaskCommentRepository taskCommentRepository) : IRequestHandler<AddTaskCommentCommand>
{
    public Task Handle(AddTaskCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new TaskComment(
            request.TaskId,
            request.UserId,
            request.Content);

        return taskCommentRepository.AddAsync(comment, cancellationToken);
    }
}
