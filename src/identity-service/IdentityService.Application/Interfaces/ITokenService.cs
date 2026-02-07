namespace IdentityService.Application.Interfaces;

public interface ITokenService
{
    Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> IssueTokenForUserAsync(
    Guid userId,
    Guid applicationId,
    Guid organizationId,
    string orgRole,
    CancellationToken ct);
}
