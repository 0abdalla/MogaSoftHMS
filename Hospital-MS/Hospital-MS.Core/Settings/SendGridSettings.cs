using System.ComponentModel.DataAnnotations;

namespace Hospital_MS.Core.Settings;
public class SendGridSettings
{
    public string ApiKey { get; set; }

    [Required, EmailAddress]
    public string FromEmail { get; set; }

    [Required]
    public string FromName { get; set; }
}
