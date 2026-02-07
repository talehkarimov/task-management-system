using IdentityService.Application.Interfaces;
using IdentityService.Domain.Models;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IdentityDbContext _db;
    public RefreshTokenRepository(IdentityDbContext db) => _db = db;

    public async Task CreateAsync(RefreshToken token, CancellationToken ct)
    {
        _db.RefreshTokens.Add(token);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<RefreshToken?> FindByHashAsync(string tokenHash, CancellationToken ct)
    {
        return await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == tokenHash, ct);
    }

    public async Task RevokeAsync(RefreshToken token, CancellationToken ct)
    {
        token.RevokedAt = DateTime.UtcNow;
        _db.RefreshTokens.Update(token);
        await _db.SaveChangesAsync(ct);
    }
}
