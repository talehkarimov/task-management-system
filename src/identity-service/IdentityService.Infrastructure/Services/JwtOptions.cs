public sealed class JwtOptions
{
    public const int DefaultAccessTokenMinutes = 60;
    public const int DefaultRefreshTokenDays = 30;
    public const int RefreshTokenBytes = 48;

    public string Issuer { get; init; } = "identity-service";
    public string Audience { get; init; } = "task-service";
    public string Secret { get; init; } = null!;
    public int AccessTokenMinutes { get; init; } = DefaultAccessTokenMinutes;
    public int RefreshTokenDays { get; init; } = DefaultRefreshTokenDays;
}