using IdentityService.Domain.Enums;

namespace IdentityService.Domain.Models;

public sealed class OrganizationMember
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid UserId { get; set; }
    public OrganizationRole Role { get; set; }
    public MembershipStatus Status { get; set; }
    public DateTime JoinedAt { get; set; }
}
