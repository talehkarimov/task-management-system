using IdentityService.Application.Commands;
using IdentityService.Application.Dtos;
using IdentityService.Application.Interfaces;
using IdentityService.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Handlers;

public sealed class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponse?>
{
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginUserHandler(IUserRepository userRepo, ITokenService tokenService, IPasswordHasher<User> passwordHasher)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepo.FindByEmailAsync(request.Email, cancellationToken);
        if (user is null) return null;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed) return null;

        var orgId = request.OrganizationId;
        var tokenResult = await _tokenService.IssueTokenForUserAsync(user.Id, Guid.Empty, orgId, "Member", cancellationToken);
        return new AuthResponse(tokenResult.AccessToken, tokenResult.RefreshToken, (int)(tokenResult.ExpiresAt - DateTime.UtcNow).TotalSeconds);
    }
}
