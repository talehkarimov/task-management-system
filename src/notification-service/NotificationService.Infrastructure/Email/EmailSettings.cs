namespace NotificationService.Infrastructure.Email;

public sealed class EmailSettings
{
    public string Host { get; init; } = null!;
    public int Port { get; init; }
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string From { get; init; } = null!;
}
