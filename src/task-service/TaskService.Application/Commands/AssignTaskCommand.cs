using MediatR;

namespace TaskService.Application.Commands;

public record AssignTaskCommand(Guid TaskId, 
    Guid AssigneeUserId, 
    Guid ChangedByUserId) : IRequest;