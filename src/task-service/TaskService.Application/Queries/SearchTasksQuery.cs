using MediatR;
using TaskService.Application.Common;
using TaskService.Application.Queries.Models;

namespace TaskService.Application.Queries;

public record SearchTasksQuery(
    string Term,
    Guid? ProjectId,
    int Page,
    int PageSize
) : IRequest<PagedResult<TaskListItemDto>>;
