namespace IdentityService.Application.Dtos;

public sealed record AuthResponse(
 string AccessToken,
 string RefreshToken,
 int ExpiresIn);
