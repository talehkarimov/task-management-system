using MediatR;

namespace TaskService.Application.Commands.Handlers;

public class AddTaskCommentHandler : IRequestHandler<AddTaskCommentCommand>
{
    public Task Handle(AddTaskCommentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
