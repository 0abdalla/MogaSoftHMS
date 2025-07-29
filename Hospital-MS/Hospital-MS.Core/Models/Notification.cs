using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models;
public class Notification
{
    public int Id { get; set; }
    public int TargetId { get; set; } // purchase Request Id - ex
    public NotificationType? Type { get; set; } // purchase Request / Dispense Request / Price Quotation
    public string? AdditionalInfo { get; set; }

    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
