using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Models;

public sealed class TokenRequest
{
    public string GrantType { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid OrganizationId { get; set; }
}
