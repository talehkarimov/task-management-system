namespace TaskService.Application.Interfaces;

public interface IIdempotencyService
{
    Task<bool> IsRequestProcessedAsync(
        string requestId,
        CancellationToken cancellationToken);

    Task MarkRequestAsProcessedAsync(
        string requestId,
        CancellationToken cancellationToken);
}
