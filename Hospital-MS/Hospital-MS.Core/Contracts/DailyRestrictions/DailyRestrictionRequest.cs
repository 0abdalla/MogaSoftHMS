using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Contracts.DailyRestrictions;
public class DailyRestrictionRequest
{
    public DateOnly RestrictionDate { get; set; }
    public int RestrictionTypeId { get; set; }
    //public string? LedgerNumber { get; set; }
    public string? Description { get; set; }
    public int? AccountingGuidanceId { get; set; } 
    public List<DailyRestrictionDetailRequest> Details { get; set; } = new();
}
