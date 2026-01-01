using Common.Constants;
using IdentityService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Domain.Models;

public sealed class User
{
    public Guid Id { get; set; }
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string Email { get; set; } = default!;
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string FirstName { get; set; } = default!;
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string LastName { get; set; } = default!;
    [MaxLength(ValidationConstants.StandartInputLength)]
    public string DisplayName { get; set; } = default!;
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
