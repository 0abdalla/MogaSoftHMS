using Hospital_MS.Core.Enums;

namespace Hospital_MS.Core.Models;
public class DailyRestriction : AuditableEntity // قيد يومية
{
    public int Id { get; set; }
    public string RestrictionNumber { get; set; } = string.Empty;
    public DateTime? RestrictionDate { get; set; }
    public AccountingGuidances AccountingGuidance { get; set; }
    public RestrictionTypes RestrictionType { get; set; }
    public string? Description { get; set; }
    public string? DocumentNumber { get; set; }
    public bool IsPosted { get; set; } = false;
    public DateTime? PostedDate { get; set; }
    public bool IsFromTransaction { get; set; } = true;
    public ICollection<DailyRestrictionDetail> Details { get; set; } = new List<DailyRestrictionDetail>();
}
