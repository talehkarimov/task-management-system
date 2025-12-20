using MediatR;
using TaskService.Application.Events;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entitites;

namespace TaskService.Application.Commands.Handlers;

public class AddTaskCommentHandler(ITaskCommentRepository taskCommentRepository, IDomainEventDispatcher eventDispatcher) : IRequestHandler<AddTaskCommentCommand>
{
    public async Task Handle(AddTaskCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = new TaskComment(
            request.TaskId,
            request.UserId,
            request.Content);

        await taskCommentRepository.AddAsync(comment, cancellationToken);

        await eventDispatcher.DispatchAsync(
            new TaskCommentAddedEvent(comment.TaskId, comment.Content, comment.UserId),
            cancellationToken
        );
    }
}
