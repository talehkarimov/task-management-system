using MediatR;

namespace TaskService.Application.Commands.Handlers;

public class BlockTaskHandler : IRequestHandler<BlockTaskCommand>
{
    public Task Handle(BlockTaskCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
