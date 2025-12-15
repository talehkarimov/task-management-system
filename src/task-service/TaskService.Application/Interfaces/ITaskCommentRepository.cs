using TaskService.Domain.Entitites;

namespace TaskService.Application.Interfaces;

public interface ITaskCommentRepository
{
    Task AddAsync(TaskComment comment, CancellationToken cancellationToken);
}
