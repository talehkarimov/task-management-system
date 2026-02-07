using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.Models;

public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ApplicationId { get; set; }
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string TokenHash { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
}
