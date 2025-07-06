using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.DailyRestrictions;
public class DailyRestrictionRequest
{
    public string RestrictionNumber { get; set; } = string.Empty;
    public DateOnly RestrictionDate { get; set; }
    public int RestrictionTypeId { get; set; }
    public string? LedgerNumber { get; set; }
    public string? Description { get; set; }
    public List<DailyRestrictionDetailRequest> Details { get; set; } = new();
}
