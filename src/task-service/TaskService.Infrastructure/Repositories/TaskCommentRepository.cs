using TaskService.Application.Interfaces;
using TaskService.Domain.Entitites;
using TaskService.Infrastructure.Persistence;

namespace TaskService.Infrastructure.Repositories;

public sealed class TaskCommentRepository(TaskDbContext dbContext) : ITaskCommentRepository
{
    public async Task AddAsync(TaskComment comment, CancellationToken cancellationToken)
    {
        await dbContext.TaskComments.AddAsync(comment, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
