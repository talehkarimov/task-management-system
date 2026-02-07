using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IdentityService.Application.Interfaces;
using IdentityService.Infrastructure.Persistence;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Services;

public sealed class JwtTokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly IdentityDbContext _db;

    public JwtTokenService(Microsoft.Extensions.Options.IOptions<JwtOptions> options, IdentityDbContext db)
    {
        _options = options.Value;
        _db = db;
    }

    public async Task<(string AccessToken, string RefreshToken, DateTime ExpiresAt)> IssueTokenForUserAsync(
    Guid userId,
    Guid applicationId,
    Guid organizationId,
    string orgRole,
    CancellationToken ct)
    {
        var claims = new List<Claim>
         {
             new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
             new Claim("org", organizationId.ToString()),
             new Claim("client_id", applicationId.ToString()),
             new Claim("org_role", orgRole)
         };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);
        var jwt = new JwtSecurityToken(
        issuer: _options.Issuer,
        audience: _options.Audience,
        claims: claims,
        expires: expiresAt,
        signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

        // generate refresh token
        var rawBytes = RandomNumberGenerator.GetBytes(JwtOptions.RefreshTokenBytes);
        var rawRefresh = Convert.ToBase64String(rawBytes);
        var hashed = ComputeSha256Hash(rawRefresh);

        // store hashed refresh token
        _db.Add(new IdentityService.Domain.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ApplicationId = applicationId,
            TokenHash = hashed,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenDays)
        });
        await _db.SaveChangesAsync(ct);

        return (accessToken, rawRefresh, expiresAt);
    }

    private static string ComputeSha256Hash(string raw)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        return Convert.ToHexString(bytes);
    }
}
