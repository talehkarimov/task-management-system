using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Common;
using TaskService.Application.Exceptions;
using TaskService.Application.Interfaces;
using TaskService.Application.Queries;
using TaskService.Application.Queries.Models;

public sealed class GetTaskByIdQueryHandler
    : IRequestHandler<GetTaskByIdQuery, TaskDetailsDto>
{
    private readonly ITaskReadDbContext _dbContext;
    private readonly ICacheService _cache;

    public GetTaskByIdQueryHandler(
        ITaskReadDbContext dbContext,
        ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task<TaskDetailsDto> Handle(
        GetTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.TaskById(request.TaskId);

        var cached = await _cache.GetAsync<TaskDetailsDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return cached;
        }

        var task = await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.Id == request.TaskId)
            .Select(t => new TaskDetailsDto
            {
                Id = t.Id,
                ProjectId = t.ProjectId,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                ReporterUserId = t.ReporterUserId,
                AssigneeUserId = t.AssigneeUserId,
                DueDate = t.DueDate
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Task not found.");

        await _cache.SetAsync(
            cacheKey,
            task,
            TimeSpan.FromMinutes(5),
            cancellationToken);

        return task;
    }
}
