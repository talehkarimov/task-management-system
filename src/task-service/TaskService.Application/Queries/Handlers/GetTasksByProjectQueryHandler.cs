using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Common;
using TaskService.Application.Interfaces;
using TaskService.Application.Queries.Models;

namespace TaskService.Application.Queries.Handlers;

public sealed class GetTasksByProjectQueryHandler 
    : IRequestHandler<GetTasksByProjectQuery, PagedResult<TaskListItemDto>>
{
    private readonly ITaskReadDbContext _dbContext;
    private readonly ICacheService _cache;
    public GetTasksByProjectQueryHandler(ITaskReadDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }
    public async Task<PagedResult<TaskListItemDto>> Handle(GetTasksByProjectQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.TasksByProject(
            request.ProjectId,
            request.Page,
            request.PageSize);

        var cached = await _cache.GetAsync<PagedResult<TaskListItemDto>>(
            cacheKey,
            cancellationToken);

        if (cached is not null)
        {
            return cached;
        }

        var query = _dbContext.Tasks
        .AsNoTracking()
        .Where(t => t.ProjectId == request.ProjectId)
        .OrderByDescending(t => t.CreatedAt)
        .Select(t => new TaskListItemDto
        {
            Id = t.Id,
            Title = t.Title,
            Status = t.Status,
            Priority = t.Priority,
            AssigneeUserId = t.AssigneeUserId,
            DueDate = t.DueDate
        });

        var result =  await query.ToPagedResultAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        await _cache.SetAsync(
            cacheKey,
            result,
            TimeSpan.FromMinutes(2),
            cancellationToken);
        return result;
    }
}
