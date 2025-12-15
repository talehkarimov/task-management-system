using MediatR;

namespace TaskService.Application.Commands;

public record ChangeTaskStatusCommand(Guid TaskId,
    Domain.Enums.TaskStatus NewStatus,
    Guid ChangedByUserId) : IRequest;
