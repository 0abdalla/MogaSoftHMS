namespace Hospital_MS.Core.Models;

public class DispensePermission : AuditableEntity
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; } = 0M;
    public string? DispenseTo { get; set; } // يصرف الي السيد:
    public string? Notes { get; set; }

    public int? TreasuryId { get; set; }
    public int? CostCenterId { get; set; }
    public int? AccountId { get; set; }

    public bool IsActive { get; set; } = true;

    public CostCenterTree? CostCenter { get; set; }
    public AccountTree? Account { get; set; }
    public Treasury? Treasury { get; set; }

    //public string Status { get; set; } = "Pending";

    public DailyRestriction? DailyRestriction { get; set; } = default!;
    public int? DailyRestrictionId { get; set; }
}