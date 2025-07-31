namespace Hospital_MS.Core.Models;
public class SupplyReceipt : AuditableEntity // ايصال التوريد
{
    public int Id { get; set; }
    public int TreasuryId { get; set; }
    public DateOnly Date { get; set; }
    public string? ReceivedFrom { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int? CostCenterId { get; set; }
    public bool IsActive { get; set; } = true;

    public int? AccountId { get; set; }
    public AccountTree? Account { get; set; }
    public Treasury Treasury { get; set; } = default!;
    public CostCenterTree CostCenter { get; set; } = default!;

    public DailyRestriction? DailyRestriction { get; set; } = default!;
    public int? DailyRestrictionId { get; set; }
}