namespace Hospital_MS.Core.Models;

public class Treasury : AuditableEntity
{
    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public bool IsActive { get; set; } = true;

    public int BranchId { get; set; }
    public Branch Branch { get; set; } = default!;

    public decimal OpeningBalance { get; set; }
    //public bool IsEnabled { get; set; } = true;

    //public DateOnly OpenedIn { get; set; }
    //public DateOnly? ClosedIn { get; set; }
}