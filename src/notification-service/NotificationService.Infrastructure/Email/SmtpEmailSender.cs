using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace NotificationService.Infrastructure.Email;

public sealed class SmtpEmailSender(IOptions<EmailSettings> options)
    : IEmailSender
{
    private readonly EmailSettings _settings = options.Value;

    public async Task SendAsync(
        string to,
        string subject,
        string body,
        CancellationToken cancellationToken)
    {
        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(
                _settings.Username,
                _settings.Password),
            EnableSsl = true
        };

        using var message = new MailMessage(
            _settings.From,
            to,
            subject,
            body);

        await client.SendMailAsync(message, cancellationToken);
    }
}
