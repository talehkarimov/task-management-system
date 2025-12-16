using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Common;
using TaskService.Application.Interfaces;
using TaskService.Application.Queries.Models;

namespace TaskService.Application.Queries.Handlers;

public sealed class SearchTasksQueryHandler
    : IRequestHandler<SearchTasksQuery, PagedResult<TaskListItemDto>>
{
    private readonly ITaskReadDbContext _dbContext;
    private readonly ICacheService _cache;

    public SearchTasksQueryHandler(ITaskReadDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<PagedResult<TaskListItemDto>> Handle(
        SearchTasksQuery request,
        CancellationToken cancellationToken)
    {
        var term = request.Term.Trim().ToLower();

        var cacheKey = CacheKeys.SearchTasks(
            term,
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
            .Where(t =>
                EF.Functions.Like(t.Title.ToLower(), $"%{term}%") ||
                (t.Description != null &&
                 EF.Functions.Like(t.Description.ToLower(), $"%{term}%")));

        if (request.ProjectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == request.ProjectId);
        }

        var projected = query
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

        var result = await projected.ToPagedResultAsync(
            request.Page,
            request.PageSize,
            cancellationToken);

        await _cache.SetAsync(
            cacheKey,
            result,
            TimeSpan.FromSeconds(30),
            cancellationToken);

        return result;
    }
}