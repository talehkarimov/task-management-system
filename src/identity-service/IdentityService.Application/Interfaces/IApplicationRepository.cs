using IdentityService.Domain.Models;

namespace IdentityService.Application.Interfaces;

public interface IApplicationRepository
{
    Task<OAuthApplication?> FindByClientIdAsync(string clientId, CancellationToken ct);
    Task CreateAsync(OAuthApplication application, CancellationToken ct);
}
