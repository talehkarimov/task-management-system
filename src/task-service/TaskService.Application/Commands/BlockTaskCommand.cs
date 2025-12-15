using MediatR;

namespace TaskService.Application.Commands;

public record BlockTaskCommand(Guid TaskId,
    string Reason,
    Guid ChangedByUserId) : IRequest;
