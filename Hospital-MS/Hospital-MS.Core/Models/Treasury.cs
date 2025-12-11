namespace Hospital_MS.Core.Models;

public class Treasury : AuditableEntity
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public AccountTree Account { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Currency { get; set; } = default!;

    public decimal OpeningBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public int BranchId { get; set; }
    public Branch Branch { get; set; } = default!;
}