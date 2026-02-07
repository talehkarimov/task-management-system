namespace IdentityService.Application.Dtos;

public sealed record RegisterUserRequest(
 string Email,
 string Password,
 string FirstName,
 string LastName,
 string DisplayName);
