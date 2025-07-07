using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_MS.Core.Models;
public class DailyRestrictionDetail
{
    public int Id { get; set; }
    public int DailyRestrictionId { get; set; }
    public DailyRestriction DailyRestriction { get; set; } = default!;

    public int AccountId { get; set; }                // الحساب (FK)
    public AccountTree Account { get; set; } = default!;  // الحساب (Navigation)
    public decimal Debit { get; set; }                // مدين
    public decimal Credit { get; set; }               // دائن
    public int? CostCenterId { get; set; }            // مركز التكلفة (FK, optional)
    public CostCenterTree? CostCenter { get; set; }       // مركز التكلفة (Navigation)
    public string? Note { get; set; }                 // ملاحظات
}