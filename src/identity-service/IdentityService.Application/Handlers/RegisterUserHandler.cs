using IdentityService.Application.Dtos;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Handlers;

public sealed class RegisterUserHandler : IRequestHandler<Commands.RegisterUserCommand, Unit>
{
    private readonly IUserRepository _repo;
    private readonly IPasswordHasher<User> _passwordHasher;

    public RegisterUserHandler(IUserRepository repo, IPasswordHasher<User> passwordHasher)
    {
        _repo = repo;
        _passwordHasher = passwordHasher;
    }

    public async Task<Unit> Handle(Commands.RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repo.FindByEmailAsync(request.Request.Email, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Request.Email,
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            DisplayName = request.Request.DisplayName,
            CreatedAt = DateTime.Now,
            Status = Domain.Enums.UserStatus.Active
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, request.Request.Password);
        await _repo.CreateAsync(user, cancellationToken);
        return Unit.Value;
    }
}
