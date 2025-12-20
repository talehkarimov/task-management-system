namespace TaskService.Application.Interfaces;

public interface IRequestContext
{
    string? CorrelationId { get; }
    Guid? UserId { get; }
    Guid? OrganizationId { get; }
}
