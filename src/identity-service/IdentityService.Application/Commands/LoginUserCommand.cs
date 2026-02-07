using IdentityService.Application.Dtos;
using MediatR;

namespace IdentityService.Application.Commands;

public sealed record LoginUserCommand(string Email, string Password, Guid OrganizationId) : IRequest<AuthResponse?>;
