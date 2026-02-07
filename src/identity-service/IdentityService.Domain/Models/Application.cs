using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.Models;

public sealed class OAuthApplication
{
    public Guid Id { get; set; }
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string ClientId { get; set; } = null!;
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string ClientSecretHash { get; set; } = null!;
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string Name { get; set; } = null!;
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string RedirectUris { get; set; } = string.Empty;
    public bool IsConfidential { get; set; }
    public DateTime CreatedAt { get; set; }
}
