namespace Hospital_MS.Core.Models;

public class RestrictionType : AuditableEntity // نوع القيد
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<DailyRestriction> DailyRestrictions { get; set; } = new List<DailyRestriction>();
}