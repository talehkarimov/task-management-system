using MediatR;

namespace TaskService.Application.Commands.Handlers;

public class CompleteTaskHandler : IRequestHandler<CompleteTaskCommand>
{
    public Task Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
