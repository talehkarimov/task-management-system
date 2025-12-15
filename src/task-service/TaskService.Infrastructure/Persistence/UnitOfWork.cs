using Microsoft.EntityFrameworkCore.Storage;
using TaskService.Application.Interfaces;

namespace TaskService.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TaskDbContext _dbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(TaskDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task BeginAsync(CancellationToken cancellationToken)
    {
        if (_transaction != null)
            return;

        _transaction = await _dbContext.Database
            .BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
            return;

        await _transaction.CommitAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction == null)
            return;

        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
        _transaction = null;
    }
}
