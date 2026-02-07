using IdentityService.Domain.Models;

namespace IdentityService.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken token, CancellationToken ct);
    Task<RefreshToken?> FindByHashAsync(string tokenHash, CancellationToken ct);
    Task RevokeAsync(RefreshToken token, CancellationToken ct);
}
