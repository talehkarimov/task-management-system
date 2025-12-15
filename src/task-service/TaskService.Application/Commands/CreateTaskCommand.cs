using MediatR;
using TaskService.Domain.Enums;

namespace TaskService.Application.Commands;

public record CreateTaskCommand(Guid ProjectId,
    string Title,
    string? Description,
    Guid ReporterUserId,
    Guid? AssigneeUserId,
    DateTime? DueDate,
    TaskPriority Priority) : IRequest<Guid>;
