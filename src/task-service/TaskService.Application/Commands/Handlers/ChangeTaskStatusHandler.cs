using MediatR;

namespace TaskService.Application.Commands.Handlers;

public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand>
{
    public Task Handle(ChangeTaskStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
