using IdentityService.Domain.Models;

namespace IdentityService.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email, CancellationToken ct);
    Task CreateAsync(User user, CancellationToken ct);
}
