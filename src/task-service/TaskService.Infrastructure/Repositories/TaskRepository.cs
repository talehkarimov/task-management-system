using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Domain.Entitites;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

public sealed class TaskRepository(TaskDbContext dbContext) : ITaskRepository
{
    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken)
    {
        await dbContext.Tasks.AddAsync(task, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Tasks
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken cancellationToken)
    {
        dbContext.Tasks.Update(task);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
