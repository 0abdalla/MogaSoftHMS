using Hospital_MS.Core.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Hospital_MS.Services.Common;
//public class SendGridEmailService(IOptions<SendGridSettings> options, ILogger<SendGridEmailService> logger) : IEmailSender
//{
//    private readonly SendGridSettings _settings = options.Value;
//    private readonly ILogger<SendGridEmailService> _logger = logger;

//    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//    {
//        var client = new SendGridClient(_settings.ApiKey);
//        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
//        var to = new EmailAddress(email);

//        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent: null, htmlContent: htmlMessage);

//        var response = await client.SendEmailAsync(msg);

//        _logger.LogInformation("Email sent to {Email}. Status: {StatusCode}", email, response.StatusCode);

//        if (!response.IsSuccessStatusCode)
//        {
//            var body = await response.Body.ReadAsStringAsync();
//            _logger.LogError("Failed to send email. Response: {Body}", body);
//        }
//    }
//}

public class SendGridEmailService : IEmailSender
{
    private readonly SendGridSettings _settings;
    private readonly ILogger<SendGridEmailService> _logger;

    public SendGridEmailService(IOptions<SendGridSettings> options, ILogger<SendGridEmailService> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
        {
            _logger.LogError("SendGrid API Key is missing. Please configure it in appsettings or environment variables.");
            throw new InvalidOperationException("SendGrid API Key is not configured.");
        }

        var client = new SendGridClient(_settings.ApiKey);

        var from = new EmailAddress(_settings.FromEmail, _settings.FromName);
        var to = new EmailAddress(email);

        var msg = MailHelper.CreateSingleEmail(
            from,
            to,
            subject,
            plainTextContent: "This email requires an HTML-compatible client.",
            htmlContent: htmlMessage
        );

        try
        {
            var response = await client.SendEmailAsync(msg);

            _logger.LogInformation("SendGrid response for {Email}: {StatusCode}", email, response.StatusCode);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Body.ReadAsStringAsync();
                _logger.LogError("SendGrid failed for {Email}. Status: {StatusCode}, Response: {ResponseBody}", email, response.StatusCode, errorBody);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while sending email to {Email}", email);
            throw;
        }
    }
}