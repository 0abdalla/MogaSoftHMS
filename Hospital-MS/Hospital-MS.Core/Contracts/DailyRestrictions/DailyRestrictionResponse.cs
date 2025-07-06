using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.DailyRestrictions;
public class DailyRestrictionResponse
{
    public int Id { get; set; }
    public string RestrictionNumber { get; set; } = string.Empty;
    public DateOnly RestrictionDate { get; set; }
    public int RestrictionTypeId { get; set; }
    public string RestrictionTypeName { get; set; } = string.Empty;
    public string? LedgerNumber { get; set; }
    public string? Description { get; set; }
    public string? CreatedBy { get; set; }
    public List<DailyRestrictionDetailResponse> Details { get; set; } = new();
}