using System.ComponentModel.DataAnnotations;

namespace Hospital_MS.Core.Models;

public class Treasury : AuditableEntity
{
    public int Id { get; set; }
    public string AccountCode { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int BranchId { get; set; }
    public string Currency { get; set; } = default!;
    public decimal OpeningBalance { get; set; }
    public bool IsActive { get; set; } = true;

    public Branch Branch { get; set; } = default!;
}