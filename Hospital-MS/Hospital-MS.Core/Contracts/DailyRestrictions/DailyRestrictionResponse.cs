using Hospital_MS.Core.Contracts.Common;

namespace Hospital_MS.Core.Contracts.DailyRestrictions;
public class DailyRestrictionResponse
{
    public int Id { get; set; }
    public string RestrictionNumber { get; set; }
    public DateOnly RestrictionDate { get; set; }
    public int? RestrictionTypeId { get; set; }
    public string? RestrictionTypeName { get; set; }

    public int? AccountingGuidanceId { get; set; }
    public string? AccountingGuidanceName { get; set; }

    //public string? LedgerNumber { get; set; }
    public string? Description { get; set; }
    public List<DailyRestrictionDetailResponse> Details { get; set; } = [];
    public AuditResponse Audit { get; set; } = new();
}