using MediatR;
using TaskService.Application.Queries.Models;

namespace TaskService.Application.Queries;

public record GetTaskByIdQuery(Guid TaskId)
    : IRequest<TaskDetailsDto>;