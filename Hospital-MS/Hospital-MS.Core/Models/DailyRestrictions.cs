using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class DailyRestriction : AuditableEntity // قيد يومية
{
    public int Id { get; set; }
    public string RestrictionNumber { get; set; } = string.Empty; 
    public DateOnly RestrictionDate { get; set; }                
    public int RestrictionTypeId { get; set; }                       
    public RestrictionType RestrictionType { get; set; } = default!;     
    //public string? LedgerNumber { get; set; }                     // رقم الدفتر
    public string? Description { get; set; }                      
    public bool IsActive { get; set; } = true;

    public int? AccountingGuidanceId { get; set; }
    public AccountingGuidance? AccountingGuidance { get; set; } = default!;
    public ICollection<DailyRestrictionDetail> Details { get; set; } = new List<DailyRestrictionDetail>();
}
