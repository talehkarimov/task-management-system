using IdentityService.Application.Interfaces;
using IdentityService.Domain.Models;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Repositories;

public sealed class ApplicationRepository : IApplicationRepository
{
    private readonly IdentityDbContext _db;
    public ApplicationRepository(IdentityDbContext db) => _db = db;

    public async Task<OAuthApplication?> FindByClientIdAsync(string clientId, CancellationToken ct)
    {
        return await _db.Applications.FirstOrDefaultAsync(x => x.ClientId == clientId, ct);
    }

    public async Task CreateAsync(OAuthApplication application, CancellationToken ct)
    {
        _db.Applications.Add(application);
        await _db.SaveChangesAsync(ct);
    }
}
