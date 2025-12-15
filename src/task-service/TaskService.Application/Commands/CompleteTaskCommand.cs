using MediatR;

namespace TaskService.Application.Commands;

public record CompleteTaskCommand(
    Guid TaskId,
    Guid CompletedByUserId
) : IRequest;
