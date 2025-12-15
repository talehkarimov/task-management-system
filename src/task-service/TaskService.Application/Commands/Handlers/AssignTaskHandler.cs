using MediatR;

namespace TaskService.Application.Commands.Handlers;

public class AssignTaskHandler : IRequestHandler<AssignTaskCommand>
{
    public Task Handle(AssignTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
