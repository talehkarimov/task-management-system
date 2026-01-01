using Common.Constants;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.Models;

public sealed class Organization
{
    public Guid Id { get; set; }
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string Name { get; set; } = default!;
    public Guid OwnerUserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
