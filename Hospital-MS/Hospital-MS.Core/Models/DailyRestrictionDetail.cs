namespace Hospital_MS.Core.Models;
public class DailyRestrictionDetail
{
    public int Id { get; set; }
    public decimal? Debit { get; set; }                // مدين
    public decimal? Credit { get; set; }               // دائن
    public string? Note { get; set; }                 // ملاحظات
    public string? From { get; set; }
    public string? To { get; set; }



    public int DailyRestrictionId { get; set; }
    public int? AccountId { get; set; }                // الحساب (FK)
    public int? CostCenterId { get; set; }            // مركز التكلفة (FK, optional)
    public DailyRestriction DailyRestriction { get; set; } = default!;
    public AccountTree? Account { get; set; } = default!;  // الحساب (Navigation)
    public CostCenterTree? CostCenter { get; set; }       // مركز التكلفة (Navigation)
}