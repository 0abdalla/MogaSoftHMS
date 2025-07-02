using Hospital_MS.Core.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Hospital_MS.Services.Common;
public class EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger) : IEmailSender
{
    private readonly MailSettings _mailSettings = mailSettings.Value; 
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _logger.LogWarning("Attempted to send email with empty address.");
            return;
        }

        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail),
            Subject = subject,
        };

        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage,
        };

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        _logger.LogInformation("Sending Email to {email}", email);

        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);

        await smtp.SendAsync(message);

        smtp.Disconnect(true);
    }
}
