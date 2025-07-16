using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital_MS.Core.Models;

[Table("TreasuryMovements",Schema ="finance")]
public class TreasuryMovement : AuditableEntity // حركة الخزينة
{
    public int Id { get; set; }
    public int TreasuryNumber { get; set; } 
    public decimal OpeningBalance { get; set; } = 0M;
    public DateOnly OpenedIn { get; set; }
    public DateOnly? ClosedIn { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsClosed { get; set; } = false;
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal Balance { get; set; }

    public int TreasuryId { get; set; }
    public Treasury Treasury { get; set; } = default!;
}