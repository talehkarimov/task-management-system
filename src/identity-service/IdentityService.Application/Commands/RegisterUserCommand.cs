using IdentityService.Application.Dtos;
using MediatR;

namespace IdentityService.Application.Commands;

public sealed record RegisterUserCommand(RegisterUserRequest Request) : IRequest<Unit>;
